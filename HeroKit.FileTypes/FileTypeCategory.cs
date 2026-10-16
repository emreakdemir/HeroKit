namespace HeroKit.FileTypes;

/// <summary>
/// The broad group of a file type.
/// </summary>
public enum FileTypeCategory
{
    /// <summary>Any other type, for example a database file.</summary>
    Other,

    /// <summary>Raster and vector images.</summary>
    Image,

    /// <summary>Documents such as PDF, Office and OpenDocument files.</summary>
    Document,

    /// <summary>Compressed archives and containers.</summary>
    Archive,

    /// <summary>Audio files.</summary>
    Audio,

    /// <summary>Video files.</summary>
    Video,

    /// <summary>Font files.</summary>
    Font,

    /// <summary>Plain text and structured text such as JSON, XML or CSV.</summary>
    Text,

    /// <summary>Native or managed programs and packages that run code: Windows executables, ELF, Mach-O, Java, Android, WebAssembly.</summary>
    Executable,

    /// <summary>Text that runs code: shell, batch, PowerShell, JavaScript, PHP and HTML.</summary>
    Script,
}
