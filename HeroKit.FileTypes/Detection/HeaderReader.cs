using System.Buffers;

namespace HeroKit.FileTypes;

// Reads the start of a stream into a pooled buffer and restores the position of a seekable stream.
internal static class HeaderReader
{
    public static T Read<T>(Stream stream, int length, HeaderFunc<T> use)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long start = stream.CanSeek ? stream.Position : 0;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);
        try
        {
            int read = stream.ReadAtLeast(buffer.AsSpan(0, length), length, throwOnEndOfStream: false);
            return use(buffer.AsSpan(0, read));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            if (stream.CanSeek)
                stream.Position = start;
        }
    }

    public static async ValueTask<T> ReadAsync<T>(Stream stream, int length, HeaderFunc<T> use, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stream);
        long start = stream.CanSeek ? stream.Position : 0;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);
        try
        {
            int read = await stream.ReadAtLeastAsync(buffer.AsMemory(0, length), length, throwOnEndOfStream: false, cancellationToken).ConfigureAwait(false);
            return use(buffer.AsSpan(0, read));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
            if (stream.CanSeek)
                stream.Position = start;
        }
    }

    public delegate T HeaderFunc<out T>(ReadOnlySpan<byte> header);
}
