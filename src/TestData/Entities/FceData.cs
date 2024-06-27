using AirWeb.Domain.Entities.Fces;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Entities;

internal static class FceData
{
    private static IEnumerable<Fce> FceSeedItems =>
    [
        new Fce(401)
        {
            Facility = DomainData.GetRandomFacility(),
            Year = 2020,
            ReviewedBy = UserData.GetUsers.ElementAt(0),
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-10).Date),
            OnsiteInspection = true,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
        new Fce(402)
        {
            Facility = DomainData.GetRandomFacility(),
            Year = 2021,
            ReviewedBy = UserData.GetUsers.ElementAt(1),
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-12).Date),
            OnsiteInspection = false,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
        new Fce(403)
        {
            Facility = DomainData.GetRandomFacility(),
            Year = 2022,
            ReviewedBy = UserData.GetUsers.ElementAt(2),
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-14).Date),
            OnsiteInspection = true,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
        },
        new Fce(404)
        {
            Facility = DomainData.GetRandomFacility(),
            Year = 2023,
            ReviewedBy = UserData.GetUsers.ElementAt(3),
            CompletedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-18).Date),
            OnsiteInspection = true,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
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
                fce.Comments.AddRange(CommentData.GetRandomCommentsList());

            return _fces;
        }
    }

    public static void ClearData() => _fces = null;
}
