namespace HeroKit.FileTypes;

public static partial class FileTypes
{
    /// <summary>Plain text: UTF-8 without control characters, or text with a UTF-16 byte order mark. Detected last, when no other type matches.</summary>
    public static FileType PlainText { get; } = new("Plain text", "text/plain", FileTypeCategory.Text, "txt", "text", "log")
    {
        Detectors = [D(Sniffers.PlainText)],
    };

    /// <summary>ZIP archive (application/zip).</summary>
    public static FileType Zip { get; } = new("ZIP archive", "application/zip", FileTypeCategory.Archive, "zip")
    {
        Detectors = [M("50 4B 03 04"), M("50 4B 05 06"), M("50 4B 07 08")],
        OtherMimeTypes = ["application/x-zip-compressed"],
    };

    /// <summary>OLE compound file: the container of legacy Office files (DOC, XLS, PPT), Outlook messages and MSI installers. The header does not show which one.</summary>
    public static FileType CompoundFile { get; } = new("OLE compound file", "application/x-ole-storage", FileTypeCategory.Document)
    {
        Detectors = [M("D0 CF 11 E0 A1 B1 1A E1")],
    };

    /// <summary>PNG image (image/png).</summary>
    public static FileType Png { get; } = new("PNG image", "image/png", FileTypeCategory.Image, "png", "apng")
    {
        Detectors = [M("89 50 4E 47 0D 0A 1A 0A")],
    };

    /// <summary>JPEG image (image/jpeg).</summary>
    public static FileType Jpeg { get; } = new("JPEG image", "image/jpeg", FileTypeCategory.Image, "jpg", "jpeg", "jpe", "jfif")
    {
        Detectors = [M("FF D8 FF")],
        OtherMimeTypes = ["image/jpg", "image/pjpeg"],
    };

    /// <summary>GIF image (image/gif).</summary>
    public static FileType Gif { get; } = new("GIF image", "image/gif", FileTypeCategory.Image, "gif")
    {
        Detectors = [T("GIF87a"), T("GIF89a")],
    };

    /// <summary>WebP image (image/webp).</summary>
    public static FileType WebP { get; } = new("WebP image", "image/webp", FileTypeCategory.Image, "webp")
    {
        Detectors = [M("52 49 46 46 ?? ?? ?? ?? 57 45 42 50")],
    };

    /// <summary>BMP image (image/bmp).</summary>
    public static FileType Bmp { get; } = new("BMP image", "image/bmp", FileTypeCategory.Image, "bmp", "dib")
    {
        Detectors = [M("42 4D ?? ?? ?? ?? 00 00 00 00")],
        OtherMimeTypes = ["image/x-ms-bmp"],
    };

    /// <summary>TIFF image (image/tiff).</summary>
    public static FileType Tiff { get; } = new("TIFF image", "image/tiff", FileTypeCategory.Image, "tif", "tiff")
    {
        Detectors = [M("49 49 2A 00"), M("4D 4D 00 2A")],
    };

    /// <summary>Windows icon (image/vnd.microsoft.icon).</summary>
    public static FileType Icon { get; } = new("Windows icon", "image/vnd.microsoft.icon", FileTypeCategory.Image, "ico")
    {
        Detectors = [M("00 00 01 00 ?? 00")],
        OtherMimeTypes = ["image/x-icon"],
    };

    /// <summary>Photoshop document (image/vnd.adobe.photoshop).</summary>
    public static FileType Psd { get; } = new("Photoshop document", "image/vnd.adobe.photoshop", FileTypeCategory.Image, "psd")
    {
        Detectors = [T("8BPS")],
    };

    /// <summary>AVIF image (image/avif).</summary>
    public static FileType Avif { get; } = new("AVIF image", "image/avif", FileTypeCategory.Image, "avif")
    {
        Detectors = [new IsoMediaDetector("avif", "avis") { IncludeCompatibleBrands = true }],
    };

    /// <summary>HEIC image (image/heic).</summary>
    public static FileType Heic { get; } = new("HEIC image", "image/heic", FileTypeCategory.Image, "heic")
    {
        Detectors = [new IsoMediaDetector("heic", "heix", "hevc", "hevx", "heim", "heis") { IncludeCompatibleBrands = true }],
    };

    /// <summary>HEIF image (image/heif).</summary>
    public static FileType Heif { get; } = new("HEIF image", "image/heif", FileTypeCategory.Image, "heif")
    {
        Detectors = [new IsoMediaDetector("mif1", "msf1")],
    };

    /// <summary>JPEG XL image (image/jxl).</summary>
    public static FileType JpegXl { get; } = new("JPEG XL image", "image/jxl", FileTypeCategory.Image, "jxl")
    {
        Detectors = [M("FF 0A"), M("00 00 00 0C 4A 58 4C 20 0D 0A 87 0A")],
    };

    /// <summary>PDF document (application/pdf).</summary>
    public static FileType Pdf { get; } = new("PDF document", "application/pdf", FileTypeCategory.Document, "pdf")
    {
        Detectors = [D(Sniffers.Pdf)],
    };

    /// <summary>Rich Text Format document (application/rtf).</summary>
    public static FileType Rtf { get; } = new("Rich Text Format document", "application/rtf", FileTypeCategory.Document, "rtf")
    {
        Detectors = [T("{\\rtf")],
        OtherMimeTypes = ["text/rtf"],
    };

    /// <summary>Word 97-2003 document (application/msword).</summary>
    public static FileType Doc { get; } = new("Word 97-2003 document", "application/msword", FileTypeCategory.Document, "doc", "dot")
    {
        Parent = CompoundFile,
    };

    /// <summary>Excel 97-2003 workbook (application/vnd.ms-excel).</summary>
    public static FileType Xls { get; } = new("Excel 97-2003 workbook", "application/vnd.ms-excel", FileTypeCategory.Document, "xls", "xlt")
    {
        Parent = CompoundFile,
    };

    /// <summary>PowerPoint 97-2003 presentation (application/vnd.ms-powerpoint).</summary>
    public static FileType Ppt { get; } = new("PowerPoint 97-2003 presentation", "application/vnd.ms-powerpoint", FileTypeCategory.Document, "ppt", "pps", "pot")
    {
        Parent = CompoundFile,
    };

    /// <summary>Outlook message (application/vnd.ms-outlook).</summary>
    public static FileType Msg { get; } = new("Outlook message", "application/vnd.ms-outlook", FileTypeCategory.Document, "msg")
    {
        Parent = CompoundFile,
    };

    /// <summary>Word document (application/vnd.openxmlformats-officedocument.wordprocessingml.document).</summary>
    public static FileType Docx { get; } = new("Word document", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", FileTypeCategory.Document, "docx", "docm", "dotx")
    {
        Detectors = [new ZipEntryDetector("word/")],
        Parent = Zip,
    };

    /// <summary>Excel workbook (application/vnd.openxmlformats-officedocument.spreadsheetml.sheet).</summary>
    public static FileType Xlsx { get; } = new("Excel workbook", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileTypeCategory.Document, "xlsx", "xlsm", "xltx")
    {
        Detectors = [new ZipEntryDetector("xl/")],
        Parent = Zip,
    };

    /// <summary>PowerPoint presentation (application/vnd.openxmlformats-officedocument.presentationml.presentation).</summary>
    public static FileType Pptx { get; } = new("PowerPoint presentation", "application/vnd.openxmlformats-officedocument.presentationml.presentation", FileTypeCategory.Document, "pptx", "pptm", "ppsx")
    {
        Detectors = [new ZipEntryDetector("ppt/")],
        Parent = Zip,
    };

    /// <summary>OpenDocument text (application/vnd.oasis.opendocument.text).</summary>
    public static FileType Odt { get; } = new("OpenDocument text", "application/vnd.oasis.opendocument.text", FileTypeCategory.Document, "odt")
    {
        Detectors = [ZipEntryDetector.ForMimeTypeEntry("application/vnd.oasis.opendocument.text")],
        Parent = Zip,
    };

    /// <summary>OpenDocument spreadsheet (application/vnd.oasis.opendocument.spreadsheet).</summary>
    public static FileType Ods { get; } = new("OpenDocument spreadsheet", "application/vnd.oasis.opendocument.spreadsheet", FileTypeCategory.Document, "ods")
    {
        Detectors = [ZipEntryDetector.ForMimeTypeEntry("application/vnd.oasis.opendocument.spreadsheet")],
        Parent = Zip,
    };

    /// <summary>OpenDocument presentation (application/vnd.oasis.opendocument.presentation).</summary>
    public static FileType Odp { get; } = new("OpenDocument presentation", "application/vnd.oasis.opendocument.presentation", FileTypeCategory.Document, "odp")
    {
        Detectors = [ZipEntryDetector.ForMimeTypeEntry("application/vnd.oasis.opendocument.presentation")],
        Parent = Zip,
    };

    /// <summary>EPUB e-book (application/epub+zip).</summary>
    public static FileType Epub { get; } = new("EPUB e-book", "application/epub+zip", FileTypeCategory.Document, "epub")
    {
        Detectors = [ZipEntryDetector.ForMimeTypeEntry("application/epub+zip")],
        Parent = Zip,
    };

    /// <summary>7-Zip archive (application/x-7z-compressed).</summary>
    public static FileType SevenZip { get; } = new("7-Zip archive", "application/x-7z-compressed", FileTypeCategory.Archive, "7z")
    {
        Detectors = [M("37 7A BC AF 27 1C")],
    };

    /// <summary>RAR archive (application/vnd.rar).</summary>
    public static FileType Rar { get; } = new("RAR archive", "application/vnd.rar", FileTypeCategory.Archive, "rar")
    {
        Detectors = [M("52 61 72 21 1A 07")],
        OtherMimeTypes = ["application/x-rar-compressed"],
    };

    /// <summary>Gzip archive (application/gzip).</summary>
    public static FileType Gzip { get; } = new("Gzip archive", "application/gzip", FileTypeCategory.Archive, "gz", "tgz")
    {
        Detectors = [M("1F 8B 08")],
        OtherMimeTypes = ["application/x-gzip"],
    };

    /// <summary>Bzip2 archive (application/x-bzip2).</summary>
    public static FileType Bzip2 { get; } = new("Bzip2 archive", "application/x-bzip2", FileTypeCategory.Archive, "bz2", "tbz2")
    {
        Detectors = [T("BZh")],
    };

    /// <summary>XZ archive (application/x-xz).</summary>
    public static FileType Xz { get; } = new("XZ archive", "application/x-xz", FileTypeCategory.Archive, "xz", "txz")
    {
        Detectors = [M("FD 37 7A 58 5A 00")],
    };

    /// <summary>Zstandard archive (application/zstd).</summary>
    public static FileType Zstandard { get; } = new("Zstandard archive", "application/zstd", FileTypeCategory.Archive, "zst")
    {
        Detectors = [M("28 B5 2F FD")],
    };

    /// <summary>Tar archive (application/x-tar).</summary>
    public static FileType Tar { get; } = new("Tar archive", "application/x-tar", FileTypeCategory.Archive, "tar")
    {
        Detectors = [T("ustar", 257)],
    };

    /// <summary>MP3 audio (audio/mpeg).</summary>
    public static FileType Mp3 { get; } = new("MP3 audio", "audio/mpeg", FileTypeCategory.Audio, "mp3")
    {
        Detectors = [T("ID3"), M("FF FB"), M("FF FA"), M("FF F3"), M("FF F2")],
        OtherMimeTypes = ["audio/mp3"],
    };

    /// <summary>AAC audio (audio/aac).</summary>
    public static FileType Aac { get; } = new("AAC audio", "audio/aac", FileTypeCategory.Audio, "aac")
    {
        Detectors = [M("FF F1"), M("FF F9")],
    };

    /// <summary>FLAC audio (audio/flac).</summary>
    public static FileType Flac { get; } = new("FLAC audio", "audio/flac", FileTypeCategory.Audio, "flac")
    {
        Detectors = [T("fLaC")],
        OtherMimeTypes = ["audio/x-flac"],
    };

    /// <summary>Ogg audio (audio/ogg).</summary>
    public static FileType Ogg { get; } = new("Ogg audio", "audio/ogg", FileTypeCategory.Audio, "ogg", "oga", "opus")
    {
        Detectors = [T("OggS")],
    };

    /// <summary>WAV audio (audio/wav).</summary>
    public static FileType Wav { get; } = new("WAV audio", "audio/wav", FileTypeCategory.Audio, "wav")
    {
        Detectors = [M("52 49 46 46 ?? ?? ?? ?? 57 41 56 45")],
        OtherMimeTypes = ["audio/x-wav", "audio/wave"],
    };

    /// <summary>MPEG-4 audio (audio/mp4).</summary>
    public static FileType M4a { get; } = new("MPEG-4 audio", "audio/mp4", FileTypeCategory.Audio, "m4a", "m4b")
    {
        Detectors = [new IsoMediaDetector("M4A ", "M4B ", "M4P ", "F4A ", "F4B ")],
        OtherMimeTypes = ["audio/x-m4a"],
    };

    /// <summary>MIDI audio (audio/midi).</summary>
    public static FileType Midi { get; } = new("MIDI audio", "audio/midi", FileTypeCategory.Audio, "mid", "midi")
    {
        Detectors = [T("MThd")],
        OtherMimeTypes = ["audio/x-midi"],
    };

    /// <summary>MPEG-4 video (iTunes) (video/x-m4v).</summary>
    public static FileType M4v { get; } = new("MPEG-4 video (iTunes)", "video/x-m4v", FileTypeCategory.Video, "m4v")
    {
        Detectors = [new IsoMediaDetector("M4V ", "M4VH", "M4VP")],
    };

    /// <summary>QuickTime video (video/quicktime).</summary>
    public static FileType QuickTime { get; } = new("QuickTime video", "video/quicktime", FileTypeCategory.Video, "mov", "qt")
    {
        Detectors = [new IsoMediaDetector("qt  ")],
    };

    /// <summary>3GPP video (video/3gpp).</summary>
    public static FileType ThreeGp { get; } = new("3GPP video", "video/3gpp", FileTypeCategory.Video, "3gp", "3g2")
    {
        Detectors = [new IsoMediaDetector("3gp4", "3gp5", "3gp6", "3gp7", "3gs7", "3ge6", "3ge7", "3gg6", "3g2a", "3g2b", "3g2c")],
        OtherMimeTypes = ["video/3gpp2"],
    };

    /// <summary>MPEG-4 video (video/mp4).</summary>
    public static FileType Mp4 { get; } = new("MPEG-4 video", "video/mp4", FileTypeCategory.Video, "mp4", "m4p")
    {
        Detectors = [new IsoMediaDetector("isom", "iso2", "iso3", "iso4", "iso5", "iso6", "mp41", "mp42", "avc1", "dash", "mmp4", "MSNV", "NDAS", "f4v ", "XAVC")],
    };

    /// <summary>WebM video (video/webm).</summary>
    public static FileType WebM { get; } = new("WebM video", "video/webm", FileTypeCategory.Video, "webm")
    {
        Detectors = [D(Sniffers.WebM)],
    };

    /// <summary>Matroska video (video/x-matroska).</summary>
    public static FileType Matroska { get; } = new("Matroska video", "video/x-matroska", FileTypeCategory.Video, "mkv", "mka", "mk3d")
    {
        Detectors = [M("1A 45 DF A3")],
    };

    /// <summary>AVI video (video/x-msvideo).</summary>
    public static FileType Avi { get; } = new("AVI video", "video/x-msvideo", FileTypeCategory.Video, "avi")
    {
        Detectors = [M("52 49 46 46 ?? ?? ?? ?? 41 56 49 20")],
    };

    /// <summary>MPEG video (video/mpeg).</summary>
    public static FileType Mpeg { get; } = new("MPEG video", "video/mpeg", FileTypeCategory.Video, "mpg", "mpeg", "m2p", "vob")
    {
        Detectors = [M("00 00 01 BA"), M("00 00 01 B3")],
    };

    /// <summary>Flash video (video/x-flv).</summary>
    public static FileType Flv { get; } = new("Flash video", "video/x-flv", FileTypeCategory.Video, "flv")
    {
        Detectors = [M("46 4C 56 01")],
    };

    /// <summary>WOFF font (font/woff).</summary>
    public static FileType Woff { get; } = new("WOFF font", "font/woff", FileTypeCategory.Font, "woff")
    {
        Detectors = [T("wOFF")],
    };

    /// <summary>WOFF2 font (font/woff2).</summary>
    public static FileType Woff2 { get; } = new("WOFF2 font", "font/woff2", FileTypeCategory.Font, "woff2")
    {
        Detectors = [T("wOF2")],
    };

    /// <summary>TrueType font (font/ttf).</summary>
    public static FileType TrueType { get; } = new("TrueType font", "font/ttf", FileTypeCategory.Font, "ttf")
    {
        Detectors = [M("00 01 00 00 00")],
    };

    /// <summary>OpenType font (font/otf).</summary>
    public static FileType OpenType { get; } = new("OpenType font", "font/otf", FileTypeCategory.Font, "otf")
    {
        Detectors = [T("OTTO")],
    };

    /// <summary>Windows executable (application/vnd.microsoft.portable-executable).</summary>
    public static FileType WindowsExecutable { get; } = new("Windows executable", "application/vnd.microsoft.portable-executable", FileTypeCategory.Executable, "exe", "dll", "sys", "scr", "com", "cpl", "ocx")
    {
        Detectors = [T("MZ")],
        OtherMimeTypes = ["application/x-msdownload", "application/x-dosexec"],
    };

    /// <summary>ELF executable (application/x-executable).</summary>
    public static FileType Elf { get; } = new("ELF executable", "application/x-executable", FileTypeCategory.Executable, "elf", "so")
    {
        Detectors = [M("7F 45 4C 46")],
        OtherMimeTypes = ["application/x-elf", "application/x-sharedlib"],
    };

    /// <summary>Mach-O executable (application/x-mach-binary).</summary>
    public static FileType MachO { get; } = new("Mach-O executable", "application/x-mach-binary", FileTypeCategory.Executable, "dylib")
    {
        Detectors = [D(Sniffers.MachO)],
    };

    /// <summary>Java class file (application/java-vm).</summary>
    public static FileType JavaClass { get; } = new("Java class file", "application/java-vm", FileTypeCategory.Executable, "class")
    {
        Detectors = [D(Sniffers.JavaClass)],
    };

    /// <summary>Android Dalvik executable (application/vnd.android.dex).</summary>
    public static FileType Dex { get; } = new("Android Dalvik executable", "application/vnd.android.dex", FileTypeCategory.Executable, "dex")
    {
        Detectors = [M("64 65 78 0A")],
    };

    /// <summary>Android package (application/vnd.android.package-archive).</summary>
    public static FileType Apk { get; } = new("Android package", "application/vnd.android.package-archive", FileTypeCategory.Executable, "apk", "aab")
    {
        Detectors = [new ZipEntryDetector("AndroidManifest.xml"), new ZipEntryDetector("classes.dex")],
        Parent = Zip,
    };

    /// <summary>Java archive (application/java-archive).</summary>
    public static FileType Jar { get; } = new("Java archive", "application/java-archive", FileTypeCategory.Executable, "jar", "war", "ear")
    {
        Detectors = [new ZipEntryDetector("META-INF/")],
        Parent = Zip,
    };

    /// <summary>WebAssembly module (application/wasm).</summary>
    public static FileType WebAssembly { get; } = new("WebAssembly module", "application/wasm", FileTypeCategory.Executable, "wasm")
    {
        Detectors = [M("00 61 73 6D")],
    };

    /// <summary>Windows shortcut (application/x-ms-shortcut).</summary>
    public static FileType WindowsShortcut { get; } = new("Windows shortcut", "application/x-ms-shortcut", FileTypeCategory.Executable, "lnk")
    {
        Detectors = [M("4C 00 00 00 01 14 02 00")],
    };

    /// <summary>Windows installer (application/x-msi).</summary>
    public static FileType Msi { get; } = new("Windows installer", "application/x-msi", FileTypeCategory.Executable, "msi", "msp")
    {
        Parent = CompoundFile,
        OtherMimeTypes = ["application/x-ms-installer"],
    };

    /// <summary>Shell script. Content starting with "#!" (a shebang line) is detected as this type, whatever the interpreter.</summary>
    public static FileType ShellScript { get; } = new("Shell script", "application/x-sh", FileTypeCategory.Script, "sh", "bash", "zsh", "ksh", "csh")
    {
        Detectors = [D(Sniffers.Shebang)],
        Parent = PlainText,
        OtherMimeTypes = ["text/x-shellscript"],
    };

    /// <summary>Windows batch file (application/x-bat).</summary>
    public static FileType Batch { get; } = new("Windows batch file", "application/x-bat", FileTypeCategory.Script, "bat", "cmd")
    {
        Parent = PlainText,
        OtherMimeTypes = ["application/x-msdos-program"],
    };

    /// <summary>PowerShell script (application/x-powershell).</summary>
    public static FileType PowerShell { get; } = new("PowerShell script", "application/x-powershell", FileTypeCategory.Script, "ps1", "psm1", "psd1")
    {
        Parent = PlainText,
    };

    /// <summary>VBScript file (text/vbscript).</summary>
    public static FileType VbScript { get; } = new("VBScript file", "text/vbscript", FileTypeCategory.Script, "vbs", "vbe", "wsf", "wsh")
    {
        Parent = PlainText,
    };

    /// <summary>JavaScript (text/javascript).</summary>
    public static FileType JavaScript { get; } = new("JavaScript", "text/javascript", FileTypeCategory.Script, "js", "mjs", "cjs")
    {
        Parent = PlainText,
        OtherMimeTypes = ["application/javascript", "application/x-javascript"],
    };

    /// <summary>PHP script (application/x-httpd-php).</summary>
    public static FileType Php { get; } = new("PHP script", "application/x-httpd-php", FileTypeCategory.Script, "php", "phtml", "php3", "php4", "php5", "php7", "php8", "phar")
    {
        Parent = PlainText,
        OtherMimeTypes = ["application/x-php", "text/x-php"],
    };

    /// <summary>HTML document. HTML can run script in a browser, so it is in the <see cref="FileTypeCategory.Script"/> category and is rejected by the default upload policy.</summary>
    public static FileType Html { get; } = new("HTML document", "text/html", FileTypeCategory.Script, "html", "htm", "xhtml", "shtml", "hta")
    {
        Detectors = [D(Sniffers.Html)],
        Parent = PlainText,
        OtherMimeTypes = ["application/xhtml+xml"],
    };

    /// <summary>XML document (application/xml).</summary>
    public static FileType Xml { get; } = new("XML document", "application/xml", FileTypeCategory.Text, "xml", "xsd", "xsl", "xslt", "rss", "atom")
    {
        Detectors = [D(Sniffers.Xml)],
        Parent = PlainText,
        OtherMimeTypes = ["text/xml"],
    };

    /// <summary>SVG image. SVG is XML and can contain script; <see cref="FileTypes.CheckUpload(ReadOnlySpan{byte}, string, UploadPolicy)"/> rejects SVG with script, event handlers or external content.</summary>
    public static FileType Svg { get; } = new("SVG image", "image/svg+xml", FileTypeCategory.Image, "svg", "svgz")
    {
        Detectors = [D(Sniffers.Svg)],
        Parent = Xml,
    };

    /// <summary>JSON document (application/json).</summary>
    public static FileType Json { get; } = new("JSON document", "application/json", FileTypeCategory.Text, "json", "map", "geojson", "webmanifest")
    {
        Detectors = [D(Sniffers.Json)],
        Parent = PlainText,
    };

    /// <summary>CSV file (text/csv).</summary>
    public static FileType Csv { get; } = new("CSV file", "text/csv", FileTypeCategory.Text, "csv")
    {
        Parent = PlainText,
    };

    /// <summary>TSV file (text/tab-separated-values).</summary>
    public static FileType Tsv { get; } = new("TSV file", "text/tab-separated-values", FileTypeCategory.Text, "tsv")
    {
        Parent = PlainText,
    };

    /// <summary>Markdown document (text/markdown).</summary>
    public static FileType Markdown { get; } = new("Markdown document", "text/markdown", FileTypeCategory.Text, "md", "markdown")
    {
        Parent = PlainText,
    };

    /// <summary>YAML document (application/yaml).</summary>
    public static FileType Yaml { get; } = new("YAML document", "application/yaml", FileTypeCategory.Text, "yaml", "yml")
    {
        Parent = PlainText,
        OtherMimeTypes = ["text/yaml", "application/x-yaml"],
    };

    /// <summary>CSS stylesheet (text/css).</summary>
    public static FileType Css { get; } = new("CSS stylesheet", "text/css", FileTypeCategory.Text, "css")
    {
        Parent = PlainText,
    };

    /// <summary>iCalendar file (text/calendar).</summary>
    public static FileType Calendar { get; } = new("iCalendar file", "text/calendar", FileTypeCategory.Text, "ics", "ifb")
    {
        Parent = PlainText,
    };

    /// <summary>vCard file (text/vcard).</summary>
    public static FileType VCard { get; } = new("vCard file", "text/vcard", FileTypeCategory.Text, "vcf", "vcard")
    {
        Parent = PlainText,
        OtherMimeTypes = ["text/x-vcard"],
    };

    /// <summary>glTF model (model/gltf+json).</summary>
    public static FileType Gltf { get; } = new("glTF model", "model/gltf+json", FileTypeCategory.Other, "gltf")
    {
        Parent = Json,
    };

    /// <summary>glTF binary model (model/gltf-binary).</summary>
    public static FileType GltfBinary { get; } = new("glTF binary model", "model/gltf-binary", FileTypeCategory.Other, "glb")
    {
        Detectors = [T("glTF")],
    };

    /// <summary>SQLite database (application/vnd.sqlite3).</summary>
    public static FileType Sqlite { get; } = new("SQLite database", "application/vnd.sqlite3", FileTypeCategory.Other, "sqlite", "sqlite3", "db")
    {
        Detectors = [T("SQLite format 3\0")],
        OtherMimeTypes = ["application/x-sqlite3"],
    };

    /// <summary>Apache Parquet file (application/vnd.apache.parquet).</summary>
    public static FileType Parquet { get; } = new("Apache Parquet file", "application/vnd.apache.parquet", FileTypeCategory.Other, "parquet")
    {
        Detectors = [T("PAR1")],
    };

    // Detection order: specific types before the types they are built on, plain text last.
    private static readonly FileType[] BuiltIn =
    [
        Docx,
        Xlsx,
        Pptx,
        Odt,
        Ods,
        Odp,
        Epub,
        Apk,
        Jar,
        Zip,
        Png,
        Jpeg,
        Gif,
        WebP,
        Bmp,
        Tiff,
        Icon,
        Psd,
        Avif,
        Heic,
        Heif,
        JpegXl,
        Pdf,
        Rtf,
        CompoundFile,
        SevenZip,
        Rar,
        Gzip,
        Bzip2,
        Xz,
        Zstandard,
        Tar,
        Mp3,
        Aac,
        Flac,
        Ogg,
        Wav,
        M4a,
        Midi,
        M4v,
        QuickTime,
        ThreeGp,
        Mp4,
        WebM,
        Matroska,
        Avi,
        Mpeg,
        Flv,
        Woff,
        Woff2,
        TrueType,
        OpenType,
        WindowsExecutable,
        Elf,
        MachO,
        JavaClass,
        Dex,
        WebAssembly,
        WindowsShortcut,
        GltfBinary,
        Sqlite,
        Parquet,
        ShellScript,
        Svg,
        Html,
        Xml,
        Json,
        PlainText,
        Doc,
        Xls,
        Ppt,
        Msg,
        Msi,
        Batch,
        PowerShell,
        VbScript,
        JavaScript,
        Php,
        Csv,
        Tsv,
        Markdown,
        Yaml,
        Css,
        Calendar,
        VCard,
        Gltf,
    ];

    private static MagicNumber M(string pattern, int offset = 0) => new(pattern, offset);

    private static MagicNumber T(string text, int offset = 0) => MagicNumber.FromText(text, offset);

    private static DelegateDetector D(DelegateDetector.Match match) => new(match);
}
