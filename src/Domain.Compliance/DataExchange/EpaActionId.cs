using System.Text.RegularExpressions;

namespace AirWeb.Domain.Compliance.DataExchange;

public partial class EpaActionId
{
    public EpaActionId(string id)
    {
        if (!IsValidFormat(id)) throw new ArgumentException($"Invalid EpaActionId format: {id}", nameof(id));
        FacilityId = (FacilityId)id[12..20];
        ActionNumber = Convert.ToInt32(id[20..]);
    }

    public FacilityId FacilityId { get; }
    public int ActionNumber { get; }

    [GeneratedRegex(EpaActionIdPattern)]
    private static partial Regex EpaActionIdRegex { get; }

    // Test at https://regex101.com/
    // language:regex
    private const string EpaActionIdPattern =
        "^GA000A000013(?:777|321|3[0-1][13579]|[0-2][0-9][13579])(?!00000)[0-9]{5}(?!00000)[0-9]{5}$";

    public static bool IsValidFormat(string id) => EpaActionIdRegex.IsMatch(id);
}
