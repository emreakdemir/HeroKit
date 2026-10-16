using HeroKit.FileTypes;
using static HeroKit.Tests.FileTypes.FileTypesTests;
using FT = HeroKit.FileTypes.FileTypes;

namespace HeroKit.Tests.FileTypes;

public class UploadCheckTests
{
    private static readonly byte[] Png = Bytes("89 50 4E 47 0D 0A 1A 0A 00 00 00 0D 49 48 44 52");
    private static readonly byte[] Exe = Bytes("4D 5A 90 00 03 00 00 00");
    private static readonly byte[] Elf = Bytes("7F 45 4C 46 02 01 01 00");

    [Fact]
    public void CheckUpload_ShouldAcceptAMatchingFile()
    {
        UploadCheckResult result = FT.CheckUpload(Png, "photo.png");

        Assert.True(result.IsSafe);
        Assert.Equal(UploadRejection.None, result.Rejection);
        Assert.Same(FT.Png, result.DetectedType);
        Assert.Null(result.Message);
    }

    [Theory]
    [InlineData("", UploadRejection.InvalidFileName)]
    [InlineData(" ", UploadRejection.InvalidFileName)]
    [InlineData(".htaccess", UploadRejection.InvalidFileName)]
    [InlineData("photo.png.", UploadRejection.InvalidFileName)]
    [InlineData("photo.png ", UploadRejection.InvalidFileName)]
    [InlineData("../photo.png", UploadRejection.InvalidFileName)]
    [InlineData("dir\\photo.png", UploadRejection.InvalidFileName)]
    [InlineData("photo.png:evil.exe", UploadRejection.InvalidFileName)]
    [InlineData("photo\0.png", UploadRejection.InvalidFileName)]
    [InlineData("pho<to>.png", UploadRejection.InvalidFileName)]
    [InlineData("photo.exe", UploadRejection.DangerousExtension)]
    [InlineData("photo.PHP", UploadRejection.DangerousExtension)]
    [InlineData("web.config", UploadRejection.DangerousExtension)]
    [InlineData("shell.aspx", UploadRejection.DangerousExtension)]
    [InlineData("index.html", UploadRejection.DangerousExtension)]
    [InlineData("photo.php.png", UploadRejection.DoubleExtension)]
    [InlineData("photo.exe.png", UploadRejection.DoubleExtension)]
    [InlineData("photo.jsp.png", UploadRejection.DoubleExtension)]
    [InlineData("photo", UploadRejection.UnknownExtension)]
    [InlineData("photo.unknownext", UploadRejection.UnknownExtension)]
    [InlineData("photo.jpg", UploadRejection.ExtensionMismatch)]
    public void CheckUpload_ShouldRejectBadNames(string fileName, UploadRejection expected)
    {
        UploadCheckResult result = FT.CheckUpload(Png, fileName);

        Assert.Equal(expected, result.Rejection);
        Assert.False(result.IsSafe);
        Assert.NotNull(result.Message);
    }

    [Fact]
    public void CheckUpload_ShouldAcceptHarmlessDotsInNames()
    {
        Assert.True(FT.CheckUpload(Png, "my.holiday.photo.png").IsSafe);
        Assert.True(FT.CheckUpload(Png, "Fotoğraf 2026.PNG").IsSafe);
    }

    [Fact]
    public void CheckUpload_ShouldRejectProgramsAndScriptsByContent()
    {
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(Exe, "photo.png").Rejection);
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(Elf, "report.pdf").Rejection);
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(Text("#!/bin/sh\nrm -rf /"), "notes.txt").Rejection);
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(Text("<html><script>x</script></html>"), "notes.txt").Rejection);
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(Zip(seekable: true, ("AndroidManifest.xml", 10)), "archive.zip").Rejection);
        Assert.Same(FT.WindowsExecutable, FT.CheckUpload(Exe, "photo.png").DetectedType);
    }

    [Fact]
    public void CheckUpload_ShouldRejectEmptyAndUnknownContent()
    {
        Assert.Equal(UploadRejection.EmptyContent, FT.CheckUpload([], "photo.png").Rejection);
        Assert.Equal(UploadRejection.UnknownContent, FT.CheckUpload(Bytes("01 02 03 04"), "photo.png").Rejection);
        Assert.True(FT.CheckUpload(Bytes("01 02 03 04"), "photo.png", new UploadPolicy { AllowUnknownContent = true }).IsSafe);
        Assert.Equal(UploadRejection.UnknownContent, FT.CheckUpload(Bytes("01 02 03 04", 4), "data.csv").Rejection);
    }

    [Fact]
    public void CheckUpload_ShouldAcceptTextAndContainersForMoreSpecificExtensions()
    {
        Assert.True(FT.CheckUpload(Text("name,age\nAyşe,30"), "people.csv").IsSafe);
        Assert.True(FT.CheckUpload(Zip(seekable: true, ("word/document.xml", 10)), "report.docx").IsSafe);
        Assert.True(FT.CheckUpload(Zip(seekable: true, ("readme.txt", 10)), "report.docx").IsSafe);
        Assert.Equal(UploadRejection.ExtensionMismatch, FT.CheckUpload(Zip(seekable: true, ("word/document.xml", 10)), "photo.png").Rejection);
    }

    [Fact]
    public void CheckUpload_ShouldApplyTheAllowList()
    {
        var policy = new UploadPolicy { AllowedTypes = [FT.Jpeg, FT.Png] };

        Assert.True(FT.CheckUpload(Png, "photo.png", policy).IsSafe);
        Assert.Equal(UploadRejection.NotAllowed, FT.CheckUpload(Bytes("25 50 44 46 2D"), "report.pdf", policy).Rejection);
    }

    [Fact]
    public void CheckUpload_ShouldApplyTheNameLengthLimit()
    {
        var policy = new UploadPolicy { MaxFileNameLength = 10 };

        Assert.True(FT.CheckUpload(Png, "photo.png", policy).IsSafe);
        Assert.Equal(UploadRejection.InvalidFileName, FT.CheckUpload(Png, "long-photo.png", policy).Rejection);
    }

    [Theory]
    [InlineData("<svg xmlns=\"http://www.w3.org/2000/svg\"><circle r=\"5\"/></svg>", true)]
    [InlineData("<?xml version=\"1.0\"?>\n<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">\n<svg/>", true)]
    [InlineData("<svg><text>Only one option</text></svg>", true)]
    [InlineData("<svg><script>alert(1)</script></svg>", false)]
    [InlineData("<svg onload=\"alert(1)\"/>", false)]
    [InlineData("<svg><rect ONCLICK = 'x'/></svg>", false)]
    [InlineData("<svg><a href=\"javascript:alert(1)\"/></svg>", false)]
    [InlineData("<svg><foreignObject><div/></foreignObject></svg>", false)]
    [InlineData("<?xml version=\"1.0\"?><!DOCTYPE svg [<!ENTITY x SYSTEM \"file:///etc/passwd\">]><svg>&x;</svg>", false)]
    [InlineData("<svg><image href=\"https://tracker.example/p.png\"/></svg>", false)]
    [InlineData("<svg><use xlink:href='//evil.example/a.svg#x'/></svg>", false)]
    [InlineData("<svg><style>@import url(x.css);</style></svg>", false)]
    public void CheckUpload_ShouldScanSvgForActiveContent(string svg, bool safe)
    {
        UploadCheckResult result = FT.CheckUpload(Text(svg), "icon.svg");

        Assert.Equal(safe, result.IsSafe);
        if (!safe)
            Assert.Equal(UploadRejection.ActiveContent, result.Rejection);
    }

    [Fact]
    public void CheckUpload_ShouldScanTheWholeSvgFromAStream()
    {
        string padding = new(' ', 20_000);
        byte[] lateScript = Text($"<svg>{padding}<script>alert(1)</script></svg>");
        using var stream = new MemoryStream(lateScript);

        Assert.Equal(UploadRejection.ActiveContent, FT.CheckUpload(stream, "icon.svg").Rejection);
        Assert.Equal(0, stream.Position);
        Assert.True(FT.CheckUpload(new MemoryStream(Text($"<svg>{padding}</svg>")), "icon.svg").IsSafe);

        var small = new UploadPolicy { MaxSvgScanLength = 1000 };
        Assert.Equal(UploadRejection.ActiveContent, FT.CheckUpload(new MemoryStream(Text($"<svg>{padding}</svg>")), "icon.svg", small).Rejection);
    }

    [Fact]
    public void CheckUpload_ShouldReadStreams()
    {
        using var stream = new MemoryStream(Png);
        stream.Position = 0;

        Assert.True(FT.CheckUpload(stream, "photo.png").IsSafe);
        Assert.Equal(0, stream.Position);
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(new NonSeekableStream(new MemoryStream(Exe)), "photo.png").Rejection);
    }

    private sealed class MinimumSizeRule(int minimum) : IUploadRule
    {
        public string? Validate(ReadOnlySpan<byte> header, string fileName, FileType? detectedType) =>
            header.Length < minimum ? $"{fileName} is smaller than {minimum} bytes." : null;
    }

    [Fact]
    public void CheckUpload_ShouldRunCustomRulesLast()
    {
        var policy = new UploadPolicy { Rules = [new MinimumSizeRule(1000)] };

        UploadCheckResult result = FT.CheckUpload(Png, "photo.png", policy);
        Assert.Equal(UploadRejection.CustomRule, result.Rejection);
        Assert.Equal("photo.png is smaller than 1000 bytes.", result.Message);
        Assert.Same(FT.Png, result.DetectedType);
        Assert.Equal(UploadRejection.DangerousContent, FT.CheckUpload(Exe, "photo.png", policy).Rejection);
        Assert.Equal(UploadRejection.CustomRule, FT.CheckUpload(new MemoryStream(Png), "photo.png", policy).Rejection);
    }

    [Fact]
    public void Validator_ShouldUseItsOwnPolicyAndRegistry()
    {
        var registry = new FileTypeRegistry();
        registry.Register(new FileType("Drawing", "image/x-drawing", FileTypeCategory.Image, "drw") { Detectors = [MagicNumber.FromText("DRW1")] });
        var validator = new UploadValidator(new UploadPolicy { AllowedTypes = [registry.FromExtension("drw")!] }, registry);

        Assert.True(validator.Validate(Text("DRW1 lines"), "plan.drw").IsSafe);
        Assert.Equal(UploadRejection.NotAllowed, validator.Validate(Png, "photo.png").Rejection);
        Assert.Equal(UploadRejection.UnknownExtension, FT.CheckUpload(Text("DRW1 lines"), "plan.drw").Rejection);
        Assert.Same(registry, validator.Registry);
        Assert.Same(FT.Default, new UploadValidator().Registry);
        Assert.Same(UploadPolicy.Default, new UploadValidator().Policy);
    }

    [Fact]
    public void Validator_ShouldBlockExtensionsFromThePolicy()
    {
        Assert.Contains("aspx", UploadPolicy.DefaultBlockedExtensions);
        Assert.Same(UploadPolicy.DefaultBlockedExtensions, UploadPolicy.Default.BlockedExtensions);

        var policy = new UploadPolicy { BlockedExtensions = new HashSet<string>(UploadPolicy.DefaultBlockedExtensions) { "png" } };
        var validator = new UploadValidator(policy);

        Assert.Equal(UploadRejection.DangerousExtension, validator.Validate(Png, "photo.png").Rejection);
        Assert.Equal(UploadRejection.DoubleExtension, validator.Validate(Png, "photo.png.jpg").Rejection);

        var open = new UploadValidator(new UploadPolicy { BlockedExtensions = new HashSet<string>() });
        Assert.True(open.Validate(Png, "photo.config.png").IsSafe);
        Assert.Equal(UploadRejection.DangerousExtension, open.Validate(Png, "photo.exe").Rejection);
    }

    [Fact]
    public void CheckUpload_ShouldRejectMacrosUnlessAllowed()
    {
        byte[] docm = Zip(seekable: true, ("word/document.xml", 200), ("word/vbaProject.bin", 500));
        byte[] docx = Zip(seekable: true, ("word/document.xml", 200));

        Assert.Equal(UploadRejection.ActiveContent, FT.CheckUpload(docm, "report.docm").Rejection);
        Assert.Equal("The document contains macros.", FT.CheckUpload(docm, "report.docm").Message);
        Assert.Equal(UploadRejection.ExtensionMismatch, FT.CheckUpload(docm, "report.docx").Rejection);
        Assert.True(FT.CheckUpload(docx, "report.docx").IsSafe);

        var allowMacros = new UploadPolicy { AllowMacros = true };
        Assert.True(FT.CheckUpload(docm, "report.docm", allowMacros).IsSafe);
    }

    [Fact]
    public void CheckUpload_ShouldFindMacrosAtTheEndOfLargeFiles()
    {
        byte[] lateMacro = Zip(seekable: true, ("word/document.xml", 200), ("word/media/big.bin", 40_000), ("word/vbaProject.bin", 500));

        Assert.Equal(UploadRejection.ExtensionMismatch, FT.CheckUpload(new MemoryStream(lateMacro), "report.docx").Rejection);
        Assert.Equal(UploadRejection.ActiveContent, FT.CheckUpload(new MemoryStream(lateMacro), "report.docm").Rejection);
    }

    private sealed class FirstBytesRule : IUploadRule
    {
        public int Length { get; private set; }

        public string? Validate(ReadOnlySpan<byte> header, string fileName, FileType? detectedType)
        {
            Length = header.Length;
            return null;
        }
    }

    [Fact]
    public void CheckUpload_ShouldGiveRulesOnlyTheFirstBytes()
    {
        var rule = new FirstBytesRule();
        byte[] late = Zip(seekable: true, ("media/big.bin", 40_000), ("word/document.xml", 100));

        Assert.True(FT.CheckUpload(new MemoryStream(late), "report.docx", new UploadPolicy { Rules = [rule] }).IsSafe);
        Assert.Equal(FT.HeaderSize, rule.Length);
    }

    [Fact]
    public void CheckUpload_ShouldValidateArguments()
    {
        Assert.Throws<ArgumentNullException>(() => FT.CheckUpload(Png, "a.png", null!));
        Assert.Throws<ArgumentNullException>(() => FT.CheckUpload((Stream)null!, "a.png"));
        Assert.Throws<ArgumentNullException>(() => new UploadValidator(null!));
        Assert.Throws<ArgumentNullException>(() => new UploadValidator(UploadPolicy.Default, null!));
        Assert.Same(UploadPolicy.Default, UploadPolicy.Default);
        Assert.Equal(255, UploadPolicy.Default.MaxFileNameLength);
    }
}
