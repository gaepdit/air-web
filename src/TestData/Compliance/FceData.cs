using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Compliance;

internal static class FceData
{
    private static IEnumerable<Fce> FceSeedItems =>
    [
        new(401, (FacilityId)"00100001", 2020)
        {
            ActionNumber = 401,
            ReviewedBy = UserData.GetRandomUser(),
            CompletedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-10)),
            OnsiteInspection = true,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
        new(402, DomainData.GetRandomFacility().Id, 2021)
        {
            ActionNumber = 402,
            ReviewedBy = UserData.Users[2],
            CompletedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3).AddDays(-12)),
            OnsiteInspection = false,
            Notes = string.Empty,
        },
        new(403, DomainData.GetRandomFacility().Id, 2022)
        {
            ActionNumber = 403,
            CompletedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-2).AddDays(-14)),
            Notes = "Deleted FCE",
        },
        new(404, DomainData.GetRandomFacility().Id, 2023)
        {
            ActionNumber = 404,
            ReviewedBy = UserData.GetRandomUser(),
            CompletedDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-4).AddDays(-50)),
            OnsiteInspection = true,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Word),
        },
    ];

    private static IEnumerable<Fce>? _fces;

    public static IEnumerable<Fce> GetData
    {
        get
        {
            if (_fces is not null) return _fces;
            _fces = FceSeedItems.ToList();

            foreach (var fce in _fces)
            {
                fce.Comments.AddRange(CommentData.GetRandomCommentsList(1)
                    .Select(comment => new FceComment(comment, fce.Id)));
            }

            _fces.Single(fce => fce.Id == 403).SetDeleted(UserData.AdminUserId);
            return _fces;
        }
    }

    public static void ClearData() => _fces = null;
}
