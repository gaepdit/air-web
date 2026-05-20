using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Sbeap;

internal static class ActionItemData
{
    private static IEnumerable<ActionItem> ActionItemSeedItems =>
    [
        new(new Guid("51000000-0000-0000-0000-000000000001"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(6))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-3),
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
        new(new Guid("51000000-0000-0000-0000-000000000002"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-1),
            Notes = string.Empty,
        },
        new(new Guid("51000000-0000-0000-0000-000000000003"),
            CaseworkData.GetCases.ElementAt(0),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-2),
            Notes = "Deleted Action Item",
        },
        new(new Guid("51000000-0000-0000-0000-000000000004"),
            CaseworkData.GetCases.ElementAt(1),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-1),
            Notes = "Action Item for open case",
        },
        new(new Guid("51000000-0000-0000-0000-000000000005"),
            CaseworkData.GetCases.ElementAt(3),
            ActionItemTypeData.GetActionItemTypes.ElementAt(3))
        {
            ActionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-1),
            Notes = "Action Item for deleted case",
        },
    ];

    private static IEnumerable<ActionItem>? _actionItems;

    public static IEnumerable<ActionItem> GetActionItems
    {
        get
        {
            if (_actionItems is not null) return _actionItems;
            _actionItems = ActionItemSeedItems.ToList();
            _actionItems.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000001");
            return _actionItems;
        }
    }

    public static void ClearData() => _actionItems = null;
}
