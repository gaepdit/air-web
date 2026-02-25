using AirWeb.Domain.Core.ValueObjects;
using AirWeb.Domain.Sbeap.Entities.Contacts;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;

namespace AirWeb.TestData.Sbeap;

internal static class ContactData
{
    private static IEnumerable<Contact> ContactSeedItems =>
    [
        new(new Guid("41000000-0000-0000-0000-000000000001"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-5),
            Honorific = "Mr.",
            GivenName = SampleText.GetRandomText(SampleText.TextLength.Word),
            FamilyName = SampleText.GetRandomText(SampleText.TextLength.Word),
            Title = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Email = SampleText.ValidEmail,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            Address = AddressData.GetRandomAddress(),
            PhoneNumbers =
            {
                PhoneNumberData.Phone1,
                PhoneNumberData.Phone2,
                PhoneNumberData.Phone3,
            },
        },
        new(new Guid("41000000-0000-0000-0000-000000000002"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-4),
            Honorific = string.Empty,
            GivenName = string.Empty,
            FamilyName = string.Empty,
            Title = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Email = string.Empty,
            Notes = string.Empty,
            Address = Address.EmptyAddress,
        },
        new(new Guid("41000000-0000-0000-0000-000000000003"),
            CustomerData.GetCustomers.ElementAt(0))
        {
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-3),
            Honorific = "Ms.",
            GivenName = "Deleted",
            FamilyName = "Contact",
            Title = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Email = SampleText.ValidEmail,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            Address = AddressData.GetRandomAddress(),
            PhoneNumbers = { PhoneNumberData.Phone4 },
        },
        new(new Guid("41000000-0000-0000-0000-000000000004"),
            CustomerData.GetCustomers.ElementAt(2))
        {
            EnteredBy = UserData.GetRandomUser(),
            EnteredOn = DateTimeOffset.Now.AddDays(-2),
            Honorific = "Mx.",
            GivenName = SampleText.GetRandomText(SampleText.TextLength.Word),
            FamilyName = SampleText.GetRandomText(SampleText.TextLength.Word),
            Title = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            Email = SampleText.ValidEmail,
            Notes = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            Address = AddressData.GetRandomAddress(),
            PhoneNumbers = { PhoneNumberData.Phone5 },
        },
    ];

    private static IEnumerable<Contact>? _contacts;

    public static IEnumerable<Contact> GetContacts
    {
        get
        {
            if (_contacts is not null) return _contacts;
            _contacts = ContactSeedItems.ToList();
            _contacts.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000001");
            return _contacts;
        }
    }

    public static void ClearData() => _contacts = null;
}
