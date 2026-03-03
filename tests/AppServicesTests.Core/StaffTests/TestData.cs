using AirWeb.Domain.Core.Entities;

namespace AppServicesTests.Core.StaffTests;

public static class TestData
{
    public const string NameSearch = "Acadia";
    public const string EmailSearch = "search@example.net";
    public const string OfficeSearch = "Badlands";

    public static List<Office> OfficeData { get; } =
    [
        new(Guid.NewGuid(), SampleText.ValidName),
        new(Guid.NewGuid(), OfficeSearch),
    ];

    public static List<ApplicationUser> StaffData { get; } =
    [
        new()
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = NameSearch,
            FamilyName = SampleText.ValidName,
            Email = SampleText.ValidEmail,
            UserName = SampleText.ValidEmail.ToLowerInvariant(),
            NormalizedEmail = SampleText.ValidEmail.ToUpperInvariant(),
            NormalizedUserName = SampleText.ValidEmail.ToUpperInvariant(),
            Office = OfficeData[0],
            Active = true,
        },
        new()
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = SampleText.ValidName,
            FamilyName = NameSearch,
            Email = SampleText.ValidEmail,
            UserName = SampleText.ValidEmail.ToLowerInvariant(),
            NormalizedEmail = SampleText.ValidEmail.ToUpperInvariant(),
            NormalizedUserName = SampleText.ValidEmail.ToUpperInvariant(),
            Office = OfficeData[1],
            Active = false,
        },
        new()
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = SampleText.ValidName,
            FamilyName = SampleText.ValidName,
            Email = EmailSearch,
            UserName = EmailSearch.ToLowerInvariant(),
            NormalizedEmail = EmailSearch.ToUpperInvariant(),
            NormalizedUserName = EmailSearch.ToUpperInvariant(),
            Office = null,
            Active = true,
        },
        new()
        {
            Id = Guid.NewGuid().ToString(),
            GivenName = SampleText.ValidName,
            FamilyName = SampleText.ValidName,
            Email = SampleText.ValidEmail,
            UserName = SampleText.ValidEmail.ToLowerInvariant(),
            NormalizedEmail = SampleText.ValidEmail.ToUpperInvariant(),
            NormalizedUserName = SampleText.ValidEmail.ToUpperInvariant(),
            Office = OfficeData[1],
            Active = true,
        },
    ];
}
