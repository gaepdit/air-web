using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities;

internal static class FceData
{
    private static IEnumerable<Fce> FceSeedItems =>
    [
        new Fce(401, DomainData.GetRandomFacility().Id, 2020)
        {
            ReviewedBy = UserData.GetUsers.ElementAt(0),
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            OnsiteInspection = true,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
        new Fce(402, DomainData.GetRandomFacility().Id, 2021)
        {
            ReviewedBy = null,
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-12).Date),
            OnsiteInspection = false,
            Notes = string.Empty,
        },
        new Fce(403, DomainData.GetRandomFacility().Id, 2022)
        {
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-14).Date),
            Notes = "Deleted FCE",
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
