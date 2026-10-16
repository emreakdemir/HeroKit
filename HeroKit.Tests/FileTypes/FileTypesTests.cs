using System.IO.Compression;
using System.Text;
using HeroKit.FileTypes;

namespace HeroKit.Tests.FileTypes;

public class FileTypesTests
{
    internal static byte[] Bytes(string hex, int padTo = 64)
    {
        byte[] data = Convert.FromHexString(hex.Replace(" ", ""));
        Array.Resize(ref data, Math.Max(padTo, data.Length));
        return data;
    }

    internal static byte[] Text(string text) => Encoding.UTF8.GetBytes(text);

    internal static byte[] Zip(bool seekable, params (string Name, int Size)[] entries)
    {
        var buffer = new MemoryStream();
        Stream target = seekable ? buffer : new NonSeekableStream(buffer);
        using (var archive = new ZipArchive(target, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach ((string name, int size) in entries)
            {
                ZipArchiveEntry entry = archive.CreateEntry(name, name == "mimetype" ? CompressionLevel.NoCompression : CompressionLevel.Optimal);
                using Stream stream = entry.Open();
                byte[] content = name == "mimetype" ? Text("application/vnd.oasis.opendocument.text") : new byte[size];
                if (name != "mimetype")
                    Random.Shared.NextBytes(content);

                stream.Write(content);
            }
        }

        return buffer.ToArray();
    }

    internal static byte[] Ftyp(string major, params string[] compatible)
    {
        var box = new List<byte>();
        int size = 16 + 4 * compatible.Length;
        box.AddRange([(byte)(size >> 24), (byte)(size >> 16), (byte)(size >> 8), (byte)size]);
        box.AddRange(Text("ftyp" + major));
        box.AddRange([0, 0, 0, 0]);
        foreach (string brand in compatible)
            box.AddRange(Text(brand));

        box.AddRange(new byte[32]);
        return [.. box];
    }

    public static TheoryData<string, string> MagicSamples => new()
    {
        { "89 50 4E 47 0D 0A 1A 0A", "image/png" },
        { "FF D8 FF E0 00 10 4A 46 49 46", "image/jpeg" },
        { "47 49 46 38 39 61", "image/gif" },
        { "52 49 46 46 24 00 00 00 57 45 42 50 56 50 38 20", "image/webp" },
        { "42 4D 36 00 0C 00 00 00 00 00 36 00", "image/bmp" },
        { "49 49 2A 00 08 00 00 00", "image/tiff" },
        { "4D 4D 00 2A 00 00 00 08", "image/tiff" },
        { "00 00 01 00 01 00 10 10", "image/vnd.microsoft.icon" },
        { "38 42 50 53 00 01", "image/vnd.adobe.photoshop" },
        { "FF 0A FA 7F", "image/jxl" },
        { "00 00 00 0C 4A 58 4C 20 0D 0A 87 0A", "image/jxl" },
        { "7B 5C 72 74 66 31", "application/rtf" },
        { "D0 CF 11 E0 A1 B1 1A E1", "application/x-ole-storage" },
        { "37 7A BC AF 27 1C 00 04", "application/x-7z-compressed" },
        { "52 61 72 21 1A 07 01 00", "application/vnd.rar" },
        { "1F 8B 08 00", "application/gzip" },
        { "42 5A 68 39 31 41", "application/x-bzip2" },
        { "FD 37 7A 58 5A 00", "application/x-xz" },
        { "28 B5 2F FD", "application/zstd" },
        { "49 44 33 04 00", "audio/mpeg" },
        { "FF FB 90 00", "audio/mpeg" },
        { "FF F1 50 80", "audio/aac" },
        { "66 4C 61 43 00 00 00 22", "audio/flac" },
        { "4F 67 67 53 00 02", "audio/ogg" },
        { "52 49 46 46 24 08 00 00 57 41 56 45 66 6D 74 20", "audio/wav" },
        { "4D 54 68 64 00 00 00 06", "audio/midi" },
        { "1A 45 DF A3 9F 42 86 81 01 42 F7 81 01 42 82 84 77 65 62 6D", "video/webm" },
        { "1A 45 DF A3 A3 42 86 81 01 42 82 88 6D 61 74 72 6F 73 6B 61", "video/x-matroska" },
        { "52 49 46 46 00 10 00 00 41 56 49 20 4C 49 53 54", "video/x-msvideo" },
        { "00 00 01 BA 44 00", "video/mpeg" },
        { "46 4C 56 01 05", "video/x-flv" },
        { "77 4F 46 46 00 01 00 00", "font/woff" },
        { "77 4F 46 32 00 01 00 00", "font/woff2" },
        { "00 01 00 00 00 0E 00 80", "font/ttf" },
        { "4F 54 54 4F 00 0A", "font/otf" },
        { "4D 5A 90 00 03 00", "application/vnd.microsoft.portable-executable" },
        { "7F 45 4C 46 02 01 01", "application/x-executable" },
        { "CF FA ED FE 0C 00 00 01", "application/x-mach-binary" },
        { "CA FE BA BE 00 00 00 02", "application/x-mach-binary" },
        { "CA FE BA BE 00 00 00 41", "application/java-vm" },
        { "64 65 78 0A 30 33 35 00", "application/vnd.android.dex" },
        { "00 61 73 6D 01 00 00 00", "application/wasm" },
        { "4C 00 00 00 01 14 02 00 00 00", "application/x-ms-shortcut" },
        { "67 6C 54 46 02 00 00 00", "model/gltf-binary" },
        { "53 51 4C 69 74 65 20 66 6F 72 6D 61 74 20 33 00", "application/vnd.sqlite3" },
        { "50 41 52 31 15 04", "application/vnd.apache.parquet" },
        { "50 4B 05 06 00 00 00 00", "application/zip" },
    };

    [Theory]
    [MemberData(nameof(MagicSamples))]
    public void Detect_ShouldRecognizeMagicBytes(string hex, string mimeType)
    {
        Assert.Equal(mimeType, HeroKit.FileTypes.FileTypes.Detect(Bytes(hex))?.MimeType);
    }

    [Theory]
    [InlineData("avif", new[] { "mif1", "avif" }, "image/avif")]
    [InlineData("mif1", new[] { "mif1", "heic" }, "image/heic")]
    [InlineData("heic", new string[0], "image/heic")]
    [InlineData("mif1", new[] { "mif1" }, "image/heif")]
    [InlineData("M4A ", new[] { "isom" }, "audio/mp4")]
    [InlineData("M4V ", new[] { "isom" }, "video/x-m4v")]
    [InlineData("qt  ", new[] { "qt  " }, "video/quicktime")]
    [InlineData("3gp5", new string[0], "video/3gpp")]
    [InlineData("isom", new[] { "isom", "avc1" }, "video/mp4")]
    [InlineData("mp42", new string[0], "video/mp4")]
    public void Detect_ShouldUseIsoMediaBrands(string major, string[] compatible, string mimeType)
    {
        Assert.Equal(mimeType, HeroKit.FileTypes.FileTypes.Detect(Ftyp(major, compatible))?.MimeType);
    }

    [Fact]
    public void Detect_ShouldNotGuessUnknownIsoMediaBrands()
    {
        Assert.Null(HeroKit.FileTypes.FileTypes.Detect(Ftyp("zzzz", "yyyy")));
    }

    [Fact]
    public void Detect_ShouldFindPdfAfterJunkAndTarAtOffset257()
    {
        Assert.Same(HeroKit.FileTypes.FileTypes.Pdf, HeroKit.FileTypes.FileTypes.Detect([.. new byte[100], .. Text("%PDF-1.7\n")]));

        byte[] tar = new byte[512];
        Text("hello.txt").CopyTo(tar, 0);
        Text("ustar").CopyTo(tar, 257);
        Assert.Same(HeroKit.FileTypes.FileTypes.Tar, HeroKit.FileTypes.FileTypes.Detect(tar));
    }

    [Theory]
    [InlineData("word/document.xml", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [InlineData("xl/workbook.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [InlineData("ppt/presentation.xml", "application/vnd.openxmlformats-officedocument.presentationml.presentation")]
    [InlineData("AndroidManifest.xml", "application/vnd.android.package-archive")]
    [InlineData("META-INF/MANIFEST.MF", "application/java-archive")]
    [InlineData("readme.txt", "application/zip")]
    public void Detect_ShouldLookInsideZipFiles(string entry, string mimeType)
    {
        byte[] zip = Zip(seekable: true, ("[Content_Types].xml", 300), ("_rels/.rels", 200), (entry, 500));

        Assert.Equal(mimeType, HeroKit.FileTypes.FileTypes.Detect(zip)?.MimeType);
    }

    [Fact]
    public void Detect_ShouldReadTheOpenDocumentMimeTypeEntry()
    {
        byte[] odt = Zip(seekable: true, ("mimetype", 0), ("content.xml", 400));

        Assert.Same(HeroKit.FileTypes.FileTypes.Odt, HeroKit.FileTypes.FileTypes.Detect(odt));
    }

    [Fact]
    public void Detect_ShouldFallBackToZipWhenEntriesAreBeyondTheHeader()
    {
        byte[] late = Zip(seekable: true, ("media/big.bin", 40_000), ("word/document.xml", 100));
        byte[] streamed = Zip(seekable: false, ("[Content_Types].xml", 300), ("word/document.xml", 100));

        // The stream overloads read only the header; a span with the whole file lets the detector see every entry.
        Assert.Same(HeroKit.FileTypes.FileTypes.Zip, HeroKit.FileTypes.FileTypes.Detect(new MemoryStream(late)));
        Assert.Same(HeroKit.FileTypes.FileTypes.Docx, HeroKit.FileTypes.FileTypes.Detect(late));
        Assert.True(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(new MemoryStream(late), "report.docx"));
        Assert.Same(HeroKit.FileTypes.FileTypes.Zip, HeroKit.FileTypes.FileTypes.Detect(streamed));
        Assert.Same(HeroKit.FileTypes.FileTypes.Docx, HeroKit.FileTypes.FileTypes.Detect(Zip(seekable: false, ("word/document.xml", 100))));
    }

    [Theory]
    [InlineData("{\"name\": \"HeroKit\"}", "application/json")]
    [InlineData("  [1, 2, 3]", "application/json")]
    [InlineData("[]", "application/json")]
    [InlineData("[section]\nkey=value", "text/plain")]
    [InlineData("<?xml version=\"1.0\"?><root/>", "application/xml")]
    [InlineData("<?xml version=\"1.0\"?>\n<svg xmlns=\"http://www.w3.org/2000/svg\"/>", "image/svg+xml")]
    [InlineData("<svg viewBox=\"0 0 10 10\"></svg>", "image/svg+xml")]
    [InlineData("<!DOCTYPE html><html></html>", "text/html")]
    [InlineData("\n<HTML><body>x</body></HTML>", "text/html")]
    [InlineData("#!/bin/sh\necho hi", "application/x-sh")]
    [InlineData("name,age\nAyşe,30\n", "text/plain")]
    [InlineData("\uFEFFhello", "text/plain")]
    public void Detect_ShouldRecognizeText(string text, string mimeType)
    {
        Assert.Equal(mimeType, HeroKit.FileTypes.FileTypes.Detect(Text(text))?.MimeType);
    }

    [Fact]
    public void Detect_ShouldHandleTextEdgeCases()
    {
        byte[] cut = Text("Merhaba dünya ğ");
        Assert.Same(HeroKit.FileTypes.FileTypes.PlainText, HeroKit.FileTypes.FileTypes.Detect(cut[..^1]));
        Assert.Same(HeroKit.FileTypes.FileTypes.PlainText, HeroKit.FileTypes.FileTypes.Detect([0xFF, 0xFE, (byte)'h', 0]));
        Assert.Null(HeroKit.FileTypes.FileTypes.Detect([0x01, 0x02, 0x03, 0x04]));
        Assert.Null(HeroKit.FileTypes.FileTypes.Detect([0xC3, 0x28, 0x41]));
        Assert.Null(HeroKit.FileTypes.FileTypes.Detect([0xEF, 0xBB, 0xBF]));
        Assert.Null(HeroKit.FileTypes.FileTypes.Detect([]));
        Assert.Empty(HeroKit.FileTypes.FileTypes.DetectAll([]));
    }

    [Fact]
    public void DetectAll_ShouldReturnEveryMatchInOrder()
    {
        byte[] docx = Zip(seekable: true, ("word/document.xml", 100));

        Assert.Equal([HeroKit.FileTypes.FileTypes.Docx, HeroKit.FileTypes.FileTypes.Zip], HeroKit.FileTypes.FileTypes.DetectAll(docx));
    }

    [Fact]
    public void Detect_ShouldReadStreamsAndRestoreThePosition()
    {
        byte[] png = Bytes("89 50 4E 47 0D 0A 1A 0A", 50_000);
        using var stream = new MemoryStream(png);
        stream.Position = 0;

        Assert.Same(HeroKit.FileTypes.FileTypes.Png, HeroKit.FileTypes.FileTypes.Detect(stream));
        Assert.Equal(0, stream.Position);
        Assert.True(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(stream, "a.png"));
        Assert.Same(HeroKit.FileTypes.FileTypes.Png, HeroKit.FileTypes.FileTypes.Detect(new NonSeekableStream(new MemoryStream(png))));
    }

    [Fact]
    public async Task DetectAsync_ShouldReadStreams()
    {
        using var stream = new MemoryStream(Bytes("25 50 44 46 2D 31 2E 34"));

        Assert.Same(HeroKit.FileTypes.FileTypes.Pdf, await HeroKit.FileTypes.FileTypes.DetectAsync(stream));
        Assert.Equal(0, stream.Position);
    }

    [Theory]
    [InlineData("photo.JPG", "image/jpeg")]
    [InlineData(".jpeg", "image/jpeg")]
    [InlineData("png", "image/png")]
    [InlineData("C:\\files\\archive.tar.gz", "application/gzip")]
    [InlineData("/var/data/report.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    [InlineData("data.json", "application/json")]
    [InlineData("module.mjs", "text/javascript")]
    [InlineData("font.woff2", "font/woff2")]
    [InlineData("noextension.", "application/octet-stream")]
    [InlineData("file.unknownext", "application/octet-stream")]
    [InlineData("", "application/octet-stream")]
    [InlineData(null, "application/octet-stream")]
    public void GetMimeType_ShouldMapExtensions(string? fileName, string mimeType)
    {
        Assert.Equal(mimeType, HeroKit.FileTypes.FileTypes.GetMimeType(fileName));
    }

    [Theory]
    [InlineData("image/x-icon", "ico")]
    [InlineData("IMAGE/JPEG", "jpg")]
    [InlineData("image/jpg", "jpg")]
    [InlineData("application/json; charset=utf-8", "json")]
    [InlineData("text/xml", "xml")]
    [InlineData("application/x-unknown", null)]
    [InlineData(" ", null)]
    [InlineData(null, null)]
    public void FromMimeType_ShouldIncludeAliasesAndIgnoreParameters(string? mimeType, string? extension)
    {
        Assert.Equal(extension, HeroKit.FileTypes.FileTypes.FromMimeType(mimeType)?.Extension);
    }

    [Theory]
    [InlineData("89 50 4E 47 0D 0A 1A 0A", "photo.png", true)]
    [InlineData("89 50 4E 47 0D 0A 1A 0A", "photo.jpg", false)]
    [InlineData("4D 5A 90 00", "photo.png", false)]
    [InlineData("89 50 4E 47 0D 0A 1A 0A", "photo.unknownext", false)]
    [InlineData("01 02 03 04", "photo.png", false)]
    public void ContentMatchesExtension_ShouldCompareTypes(string hex, string fileName, bool expected)
    {
        Assert.Equal(expected, HeroKit.FileTypes.FileTypes.ContentMatchesExtension(Bytes(hex), fileName));
    }

    [Fact]
    public void ContentMatchesExtension_ShouldAcceptTextForTextExtensions()
    {
        Assert.True(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(Text("a,b\n1,2"), "data.csv"));
        Assert.True(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(Text("{\"a\":1}"), "data.txt"));
        Assert.True(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(Text("<svg></svg>"), "icon.svg"));
        Assert.False(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(Text("hello"), "photo.png"));
    }

    [Fact]
    public void BuiltInTypes_ShouldBeConsistent()
    {
        IReadOnlyList<FileType> all = HeroKit.FileTypes.FileTypes.All;

        Assert.True(all.Count >= 86);
        Assert.Same(HeroKit.FileTypes.FileTypes.PlainText, all.Last(t => t.Detectors.Count > 0));
        Assert.All(all, t => Assert.Equal(t.MimeType, t.MimeType.ToLowerInvariant()));
        Assert.Same(HeroKit.FileTypes.FileTypes.Jpeg, HeroKit.FileTypes.FileTypes.FromExtension("jpeg"));
        Assert.Equal("jpg", HeroKit.FileTypes.FileTypes.Jpeg.Extension);
        Assert.True(HeroKit.FileTypes.FileTypes.Docx.IsA(HeroKit.FileTypes.FileTypes.Zip));
        Assert.False(HeroKit.FileTypes.FileTypes.Zip.IsA(HeroKit.FileTypes.FileTypes.Docx));
        Assert.True(HeroKit.FileTypes.FileTypes.Svg.IsA(HeroKit.FileTypes.FileTypes.PlainText));
        Assert.True(HeroKit.FileTypes.FileTypes.Html.IsExecutable);
        Assert.False(HeroKit.FileTypes.FileTypes.Pdf.IsExecutable);
        Assert.Equal("image/png", HeroKit.FileTypes.FileTypes.Png.ToString());
    }

    [Fact]
    public void Register_ShouldCheckCustomTypesFirst()
    {
        var custom = new FileType("HeroKit test", "Application/X-HeroKit-Test", FileTypeCategory.Other, ".HKT1", "")
        {
            Detectors = [MagicNumber.FromText("HKT1")],
        };
        var customZip = new FileType("HeroKit project", "application/x-herokit-project", FileTypeCategory.Document, "hkproj")
        {
            Detectors = [new ZipEntryDetector("herokit.json")],
            Parent = HeroKit.FileTypes.FileTypes.Zip,
        };

        HeroKit.FileTypes.FileTypes.Register(custom);
        HeroKit.FileTypes.FileTypes.Register(customZip);

        Assert.Same(custom, HeroKit.FileTypes.FileTypes.Detect(Text("HKT1 data")));
        Assert.Same(customZip, HeroKit.FileTypes.FileTypes.Detect(Zip(seekable: true, ("herokit.json", 10))));
        Assert.Same(HeroKit.FileTypes.FileTypes.Docx, HeroKit.FileTypes.FileTypes.Detect(Zip(seekable: true, ("word/document.xml", 10))));
        Assert.Same(custom, HeroKit.FileTypes.FileTypes.FromExtension("x.hkt1"));
        Assert.Same(custom, HeroKit.FileTypes.FileTypes.FromMimeType("application/x-herokit-test"));
        Assert.Equal(["hkt1"], custom.Extensions);
        Assert.Equal("application/x-herokit-test", custom.MimeType);
        Assert.True(HeroKit.FileTypes.FileTypes.ContentMatchesExtension(Zip(seekable: true, ("herokit.json", 10)), "a.hkproj"));
        Assert.Contains(custom, HeroKit.FileTypes.FileTypes.All);
    }

    [Fact]
    public void Constructors_ShouldValidateArguments()
    {
        Assert.Throws<ArgumentException>(() => new FileType(" ", "a/b", FileTypeCategory.Other));
        Assert.Throws<ArgumentException>(() => new FileType("A", "", FileTypeCategory.Other));
        Assert.Throws<ArgumentException>(() => new MagicNumber("GG"));
        Assert.Throws<ArgumentException>(() => new MagicNumber("123"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new MagicNumber("00", -1));
        Assert.Throws<ArgumentException>(() => MagicNumber.FromText(""));
        Assert.Throws<ArgumentException>(() => new ZipEntryDetector(""));
        Assert.Throws<ArgumentException>(() => ZipEntryDetector.ForMimeTypeEntry(""));
        Assert.Throws<ArgumentException>(() => new IsoMediaDetector());
        Assert.Throws<ArgumentException>(() => new IsoMediaDetector("abc"));
        Assert.Throws<ArgumentNullException>(() => HeroKit.FileTypes.FileTypes.Register(null!));
        Assert.Throws<ArgumentNullException>(() => HeroKit.FileTypes.FileTypes.Detect((Stream)null!));
        Assert.Equal(12, new MagicNumber("?? ?? 41", 9).Length);
    }

    internal sealed class NonSeekableStream(Stream inner) : Stream
    {
        public override bool CanRead => inner.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => inner.CanWrite;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => inner.Position; set => throw new NotSupportedException(); }
        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => inner.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => inner.Write(buffer, offset, count);
    }
}
