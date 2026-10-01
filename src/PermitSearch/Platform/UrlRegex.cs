using System.Text.RegularExpressions;

namespace PermitSearch.Platform;

internal static partial class UrlRegex
{
    // language=regex
    public const string PermitFormat = "[OPV][ADFHINPT]-[0-9]{1,8}";

    [GeneratedRegex(PermitFormat)]
    private static partial Regex PermitRegex { get; }

    public static bool IsValidPermit(string id) => PermitRegex.IsMatch(id);

    // language=regex
    public const string AirsFormat = "[0-9]{1,3}-[0-9]{1,5}|[0-9]{8}";

    [GeneratedRegex(AirsFormat)]
    private static partial Regex AirsRegex { get; }

    public static bool IsValidAirs(string id) => AirsRegex.IsMatch(id);
}
