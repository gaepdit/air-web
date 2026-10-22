using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PermitSearch.Platform;

internal static partial class IdValidation
{
    // Test REGEX at https://regex101.com/r/cmxQjk/1
    // language=regex
    public const string PermitFormat = "(?:PDF-|DOC-)?[OPV][ADFHINPT]-[0-9]{1,8}";

    [GeneratedRegex(PermitFormat)]
    private static partial Regex PermitRegex { get; }

    [GeneratedRegex($"^{PermitFormat}$")]
    private static partial Regex PermitValidationRegex { get; }

    private static bool IsValidPermit(string id) => PermitValidationRegex.IsMatch(id);

    // language=regex
    private const string PermitPrefix = "^(PDF-|DOC-)";

    [GeneratedRegex(PermitPrefix)]
    private static partial Regex PermitPrefixRegex { get; }


    public static bool ValidatePermit(string id, [NotNullWhen(true)] out string? normalizedId)
    {
        if (IsValidPermit(id))
        {
            normalizedId = PermitPrefixRegex.Replace(id, string.Empty);
            return true;
        }

        normalizedId = null;
        return false;
    }

    // language=regex
    public const string AirsFormat = "[0-9]{1,3}-[0-9]{1,5}|[0-9]{8}";

    [GeneratedRegex(AirsFormat)]
    private static partial Regex AirsRegex { get; }

    private static bool IsValidAirs(string id) => AirsRegex.IsMatch(id);

    public static bool ValidateAirs(string airsNumber, [NotNullWhen(true)] out string? normalizedAirs)
    {
        if (IsValidAirs(airsNumber))
        {
            normalizedAirs = NormalizeAirs(airsNumber);
            return true;
        }

        normalizedAirs = null;
        return false;
    }

    private static string NormalizeAirs(string value)
    {
        var dashIndex = value.IndexOf('-');
        return dashIndex == -1
            ? value
            : value[..dashIndex].PadLeft(3, '0') + value[(dashIndex + 1)..].PadLeft(5, '0');
    }
}
