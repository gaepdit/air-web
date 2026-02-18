using AirWeb.Domain.Core.Data;
using AirWeb.Domain.Core.Entities.ValueObjects;
using AirWeb.Domain.Sbeap.Entities.Customers;

namespace AppServicesTests.Sbeap.TestData;

internal static class CustomerData
{
    public static IEnumerable<Customer> GetCustomers => new List<Customer>
    {
        new(new Guid("40000000-0000-0000-0000-000000000001"))
        {
            Name = Constants.Phrase,
            County = "Bacon",
            Location = new Address
            {
                City = Constants.ValidName,
            },
            MailingAddress = Address.EmptyAddress,
            Description = Constants.Phrase,
            SicCode = CommonData.SicCodes.ElementAt(0),
        },
        new(new Guid("40000000-0000-0000-0000-000000000002"))
        {
            Name = Constants.ValidName,
            County = null,
            Location = Address.EmptyAddress,
            MailingAddress = Address.EmptyAddress,
            Description = Constants.Phrase,
        },
        new(new Guid("40000000-0000-0000-0000-000000000003"))
        {
            Name = "A Deleted Customer",
            County = null,
            Location = Address.EmptyAddress,
            MailingAddress = Address.EmptyAddress,
            Description = Constants.Phrase,
            SicCode = CommonData.SicCodes.First(),
            Website = null,
            DeleteComments = Constants.Phrase,
        },
    };
}
