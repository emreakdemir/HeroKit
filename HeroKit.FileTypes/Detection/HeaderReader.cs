using System.Buffers;

namespace HeroKit.FileTypes;

// Reads the bytes used for detection from a stream: the first HeaderSize bytes and, for a seekable ZIP file longer than that,
// its central directory after them, so ZipEntryDetector sees every entry name. The returned buffer is rented and must be returned.
internal static class HeaderReader
{
    public static T Read<T>(Stream stream, HeaderFunc<T> use)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long start = stream.CanSeek ? stream.Position : 0;
        DetectionBytes bytes = ReadDetectionBytes(stream, start);
        try
        {
            return use(bytes.Span);
        }
        finally
        {
            bytes.Return();
            if (stream.CanSeek)
                stream.Position = start;
        }
    }

    public static async ValueTask<T> ReadAsync<T>(Stream stream, HeaderFunc<T> use, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long start = stream.CanSeek ? stream.Position : 0;
        DetectionBytes bytes = await ReadDetectionBytesAsync(stream, start, cancellationToken).ConfigureAwait(false);
        try
        {
            return use(bytes.Span);
        }
        finally
        {
            bytes.Return();
            if (stream.CanSeek)
                stream.Position = start;
        }
    }

    // Leaves a non-ZIP stream positioned right after the header, so callers can read on.
    public static DetectionBytes ReadDetectionBytes(Stream stream, long start)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(FileTypeRegistry.HeaderSize);
        int read = stream.ReadAtLeast(buffer.AsSpan(0, FileTypeRegistry.HeaderSize), FileTypeRegistry.HeaderSize, throwOnEndOfStream: false);
        if (!CanHaveMoreEntries(stream, start, buffer, read))
            return new DetectionBytes(buffer, read, read);

        long fileLength = stream.Length - start;
        byte[] tail = ArrayPool<byte>.Shared.Rent(ZipDirectoryLocator.MaxTailLength);
        try
        {
            int tailLength = (int)Math.Min(fileLength, ZipDirectoryLocator.MaxTailLength);
            stream.Position = start + fileLength - tailLength;
            stream.ReadExactly(tail, 0, tailLength);
            if (!ZipDirectoryLocator.TryLocate(tail.AsSpan(0, tailLength), fileLength, out long offset, out int length))
                return new DetectionBytes(buffer, read, read);

            byte[] combined = Combine(buffer, read, length);
            stream.Position = start + offset;
            stream.ReadExactly(combined, read, length);
            return new DetectionBytes(combined, read + length, read);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(tail);
        }
    }

    public static async ValueTask<DetectionBytes> ReadDetectionBytesAsync(Stream stream, long start, CancellationToken cancellationToken)
    {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(FileTypeRegistry.HeaderSize);
        int read = await stream.ReadAtLeastAsync(buffer.AsMemory(0, FileTypeRegistry.HeaderSize), FileTypeRegistry.HeaderSize, throwOnEndOfStream: false, cancellationToken).ConfigureAwait(false);
        if (!CanHaveMoreEntries(stream, start, buffer, read))
            return new DetectionBytes(buffer, read, read);

        long fileLength = stream.Length - start;
        byte[] tail = ArrayPool<byte>.Shared.Rent(ZipDirectoryLocator.MaxTailLength);
        try
        {
            int tailLength = (int)Math.Min(fileLength, ZipDirectoryLocator.MaxTailLength);
            stream.Position = start + fileLength - tailLength;
            await stream.ReadExactlyAsync(tail.AsMemory(0, tailLength), cancellationToken).ConfigureAwait(false);
            if (!ZipDirectoryLocator.TryLocate(tail.AsSpan(0, tailLength), fileLength, out long offset, out int length))
                return new DetectionBytes(buffer, read, read);

            byte[] combined = Combine(buffer, read, length);
            stream.Position = start + offset;
            await stream.ReadExactlyAsync(combined.AsMemory(read, length), cancellationToken).ConfigureAwait(false);
            return new DetectionBytes(combined, read + length, read);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(tail);
        }
    }

    public delegate T HeaderFunc<out T>(ReadOnlySpan<byte> bytes);

    private static bool CanHaveMoreEntries(Stream stream, long start, byte[] buffer, int read) =>
        read == FileTypeRegistry.HeaderSize && stream.CanSeek && stream.Length - start > read
        && ZipDirectoryLocator.StartsLikeZip(buffer.AsSpan(0, read));

    // Copies the header into a larger rented buffer and returns the header buffer to the pool.
    private static byte[] Combine(byte[] header, int headerLength, int extra)
    {
        byte[] combined = ArrayPool<byte>.Shared.Rent(headerLength + extra);
        header.AsSpan(0, headerLength).CopyTo(combined);
        ArrayPool<byte>.Shared.Return(header);
        return combined;
    }
}

// Rented bytes for detection: the header, optionally followed by the ZIP central directory.
internal readonly struct DetectionBytes(byte[] buffer, int length, int headerLength)
{
    // Everything read for detection.
    public ReadOnlySpan<byte> Span => buffer.AsSpan(0, length);

    // Only the first bytes of the file.
    public ReadOnlySpan<byte> Header => buffer.AsSpan(0, headerLength);

    public bool HasDirectory => length > headerLength;

    public void Return() => ArrayPool<byte>.Shared.Return(buffer);
}
