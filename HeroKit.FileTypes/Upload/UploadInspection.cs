namespace HeroKit.FileTypes;

// The state one upload check passes along the chain of steps. Steps read it and fill in what they learn.
internal ref struct UploadInspection
{
    public UploadInspection(ReadOnlySpan<byte> content, ReadOnlySpan<byte> header, bool contentTruncated, string fileName, UploadPolicy policy, FileTypeRegistry registry)
    {
        Content = content;
        Header = header;
        ContentTruncated = contentTruncated;
        FileName = fileName;
        Policy = policy;
        Registry = registry;
    }

    // The bytes to scan: the whole file for SVG read from a stream, otherwise what the caller passed.
    public ReadOnlySpan<byte> Content { get; }

    // The first bytes used for detection, at most FileTypeRegistry.HeaderSize.
    public ReadOnlySpan<byte> Header { get; }

    // True when the file is longer than the content that was read for scanning.
    public bool ContentTruncated { get; }

    public string FileName { get; }

    public UploadPolicy Policy { get; }

    public FileTypeRegistry Registry { get; }

    // The parts of the file name split at dots; filled by the file name step.
    public string[] NameParts { get; set; } = [];

    // The last extension in lower case, or null; filled by the file name step.
    public string? Extension { get; set; }

    // The type of the extension; filled by the extension step.
    public FileType? ExtensionType { get; set; }

    // The type detected from the content; filled by the detection step.
    public FileType? DetectedType { get; set; }

    public readonly UploadCheckResult Reject(UploadRejection rejection, string message) => new(rejection, DetectedType, message);

    public readonly UploadCheckResult Reject(UploadRejection rejection, FileType? detectedType, string message) => new(rejection, detectedType, message);
}

// One link of the upload check chain (Chain of Responsibility): returns a rejection to stop, or null to pass to the next step.
internal interface IUploadStep
{
    UploadCheckResult? Check(ref UploadInspection inspection);
}
