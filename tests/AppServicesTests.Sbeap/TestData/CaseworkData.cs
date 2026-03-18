using AirWeb.Domain.Sbeap.Entities.Cases;

namespace AppServicesTests.Sbeap.TestData;

internal static class CaseworkData
{
    public static IEnumerable<Casework> GetCases => new List<Casework>
    {
        new(new Guid("50000000-0000-0000-0000-000000000001"),
            CustomerData.GetCustomers.ElementAt(0),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-4)))
        {
            CaseClosedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
            Description = Constants.Phrase,
        },
        new(new Guid("50000000-0000-0000-0000-000000000002"),
            CustomerData.GetCustomers.ElementAt(0),
            DateOnly.FromDateTime(DateTime.Today).AddDays(-1))
        {
            CaseClosedDate = null,
            Description = Constants.Phrase,
        },
        new(new Guid("50000000-0000-0000-0000-000000000003"),
            CustomerData.GetCustomers.ElementAt(2),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-10)))
        {
            CaseClosedDate = null,
            Description = "An open case for a deleted customer.",
        },
        new(new Guid("50000000-0000-0000-0000-000000000004"),
            CustomerData.GetCustomers.ElementAt(1),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-10)))
        {
            Description = "A Deleted Case",
        },
    };
}
