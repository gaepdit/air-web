using AirWeb.Domain.Core.Data;
using AirWeb.Domain.Core.ValueObjects;
using AirWeb.Domain.Sbeap.Entities.Customers;
using AirWeb.TestData.SampleData;
using IaipDataService.TestData;

namespace AirWeb.TestData.Sbeap;

internal static class CustomerData
{
    private static IEnumerable<Customer> CustomerSeedItems =>
    [
        new(new Guid("40000000-0000-0000-0000-000000000001"))
        {
            Name = SampleText.GetRandomText(SampleText.TextLength.Phrase),
            County = "Bacon",
            Location = AddressData.GetRandomAddress(),
            MailingAddress = AddressData.GetRandomAddress(),
            Description = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            SicCode = SicCodes.Data.First(),
            Website = SampleText.ValidUrl,
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"))
        {
            Name = SampleText.GetRandomText(SampleText.TextLength.Word),
            County = null,
            Location = Address.EmptyAddress,
            MailingAddress = Address.EmptyAddress,
            Description = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
            Website = null,
        },
        new(new Guid("40000000-0000-0000-0000-000000000003"))
        {
            Name = "A Deleted Customer",
            County = null,
            Location = AddressData.GetRandomAddress(),
            MailingAddress = Address.EmptyAddress,
            Description = TextData.ShortMultiline,
            SicCode = SicCodes.Data.First(),
            Website = null,
            DeleteComments = TextData.ShortMultiline,
        },
    ];

    private static IEnumerable<Customer>? _customers;

    public static IEnumerable<Customer> GetCustomers
    {
        get
        {
            if (_customers is not null) return _customers;
            _customers = CustomerSeedItems.ToList();
            _customers.ElementAt(2).SetDeleted("00000000-0000-0000-0000-000000000002");
            return _customers;
        }
    }

    public static void ClearData() => _customers = null;
}
