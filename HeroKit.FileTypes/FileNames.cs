namespace HeroKit.FileTypes;

// File name parsing shared by lookups and upload checks.
internal static class FileNames
{
    // The text after the last dot of the file name, in lower case; a value without a dot is taken as an extension.
    public static string? GetExtension(string? fileNameOrExtension)
    {
        if (string.IsNullOrWhiteSpace(fileNameOrExtension))
            return null;

        string value = fileNameOrExtension.Trim();
        int separator = value.LastIndexOfAny(['/', '\\']);
        string name = separator >= 0 ? value[(separator + 1)..] : value;
        int dot = name.LastIndexOf('.');
        string extension = dot >= 0 ? name[(dot + 1)..] : name;
        return extension.Length == 0 ? null : extension.ToLowerInvariant();
    }
}
