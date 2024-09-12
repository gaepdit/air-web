using System.Diagnostics.CodeAnalysis;

namespace AirWeb.TestData.SampleData;

[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded")]
public static class SampleText
{
    // Text constants
    public const string ValidName = "abc";
    public const string ShortName = "z";
    public const string NewValidName = "def";
    public const string NonExistentName = "zzz";
    public const string ValidEmail = "test@example.net";
    public const string ValidUrl = "https://example.net";
    public const string ValidPhoneNumber = "404-555-1212";
    public const string ValidFacilityId = "00109999";
    public const int ValidReferenceNumber = 202209999;
    public static Guid UnassignedGuid => new Guid("99999999-0000-0000-0000-999999999999");

    // Words and phrases
    public enum TextLength
    {
        Word,
        Phrase,
        Paragraph,
    }

    public static string GetRandomText(TextLength length, bool allowEmpty = false) =>
        allowEmpty
            ? TextAllowEmpty[length][new Random().Next(TextAllowEmpty[length].Length)]
            : Text[length][new Random().Next(Text[length].Length)];

    private static readonly Dictionary<TextLength, string[]> Text = new()
    {
        {
            TextLength.Word, ["Astonishment", "Cosmos🌠", "extraordinary", "AWE"]
        },
        {
            TextLength.Phrase,
            [
                "Mauris varius",
                "Sed ornare dui eu lectus laoreet \ud83c\udf89 egestas.",
                "Nulla pulvinar metus ut enim fermentum.",
                "Emoji Factory: \u2764\ufe0f\n\u2728\n\ud83d\udd25\n\u2705\n\ud83d\udc80\n\ud83d\ude2d\n\ud83d\ude0a\n\ud83d\ude02\n\u2600\ufe0f",
            ]
        },
        {
            TextLength.Paragraph,
            [
                "Donec et tristique metus, et scelerisque dolor. Duis vitae venenatis augue, id fringilla turpis. " +
                "Donec libero metus, imperdiet eget congue vel, malesuada eget nibh. Nullam tincidunt turpis libero, " +
                "ac tempus dolor efficitur ac. In suscipit lectus eget nisi aliquet venenatis. Lorem ipsum dolor sit " +
                "amet, consectetur adipiscing elit. Quisque viverra tristique convallis.",
                "Curabitur tristique in massa commodo maximus. Pellentesque id finibus augue. Integer vitae rhoncus " +
                "ex, eget tincidunt lorem. Etiam non erat lorem. In hac habitasse platea dictumst. Aenean ac tellus " +
                "consectetur, volutpat dolor a, sollicitudin mauris. Integer eu nibh facilisis, sagittis tellus sit " +
                "amet, mollis erat. Nam non ipsum nisl. Orci varius natoque penatibus et magnis dis parturient " +
                "montes, nascetur ridiculus mus. Vestibulum id diam ac elit fringilla ultricies vitae quis nulla. " +
                "Etiam finibus porta placerat. Donec vel dui commodo, lobortis magna sit amet, consequat eros.",
                "Suspendisse quis viverra ligula, nec cursus nulla. Curabitur elit sodales, vehicula nisl efficitur.",
                "Proin aliquet dolor vel pulvinar luctus. Curabitur vel tortor eu felis gravida dignissim. Mauris " +
                "\ud83d\udc4d \ud83e\udd79 \ud83d\ude80 " +
                "pretium dictum sagittis. Donec faucibus cursus sem, varius lacinia turpis elementum eu. Sed " +
                "fermentum, enim sed sagittis congue, ex magna dignissim magna, a semper elit risus malesuada sapien.",
            ]
        },
    };

    private static readonly Dictionary<TextLength, string[]> TextAllowEmpty = new()
    {
        {
            TextLength.Word, ["Astonishment", "Cosmos🌠", "extraordinary", "AWE", ""]
        },
        {
            TextLength.Phrase,
            [
                "Mauris varius",
                "Sed ornare dui eu lectus laoreet \ud83c\udf89 egestas.",
                "Nulla pulvinar metus ut enim fermentum.",
                "\u2764\ufe0f\n\u2728\n\ud83d\udd25\n\u2705\n\ud83d\udc80\n\ud83d\ude2d\n\ud83d\ude0a\n\ud83d\ude02\n\u2600\ufe0f",
                "",
            ]
        },
        {
            TextLength.Paragraph,
            [
                "Donec et tristique metus, et scelerisque dolor. Duis vitae venenatis augue, id fringilla turpis. " +
                "Donec libero metus, imperdiet eget congue vel, malesuada eget nibh. Nullam tincidunt turpis libero, " +
                "ac tempus dolor efficitur ac. In suscipit lectus eget nisi aliquet venenatis. Lorem ipsum dolor sit " +
                "amet, consectetur adipiscing elit. Quisque viverra tristique convallis.",
                "Curabitur tristique in massa commodo maximus. Pellentesque id finibus augue. Integer vitae rhoncus " +
                "ex, eget tincidunt lorem. Etiam non erat lorem. In hac habitasse platea dictumst. Aenean ac tellus " +
                "consectetur, volutpat dolor a, sollicitudin mauris. Integer eu nibh facilisis, sagittis tellus sit " +
                "amet, mollis erat. Nam non ipsum nisl. Orci varius natoque penatibus et magnis dis parturient " +
                "montes, nascetur ridiculus mus. Vestibulum id diam ac elit fringilla ultricies vitae quis nulla. " +
                "Etiam finibus porta placerat. Donec vel dui commodo, lobortis magna sit amet, consequat eros.",
                "Suspendisse quis viverra ligula, nec cursus nulla. Curabitur elit sodales, vehicula nisl efficitur.",
                "Proin aliquet dolor vel pulvinar luctus. Curabitur vel tortor eu felis gravida dignissim. Mauris " +
                "\ud83d\udc4d \ud83e\udd79 \ud83d\ude80 " +
                "pretium dictum sagittis. Donec faucibus cursus sem, varius lacinia turpis elementum eu. Sed " +
                "fermentum, enim sed sagittis congue, ex magna dignissim magna, a semper elit risus malesuada sapien.",
                "",
            ]
        },
    };
}
