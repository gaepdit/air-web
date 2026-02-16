using AirWeb.Domain.Core.Entities.ValueObjects;

namespace AirWeb.TestData.SampleData;

public static class AddressData
{
    public static Address GetRandomAddress() => new[] { CompleteAddress, LessCompleteAddress, IncompleteAddress }
        [Random.Shared.Next(3)];

    private static Address CompleteAddress => new()
    {
        Street = "123 Main St.",
        Street2 = "Box 456",
        City = "Town-ville",
        PostalCode = "98765-1234",
        State = "Georgia",
    };

    private static Address LessCompleteAddress => new()
    {
        Street = "456 Second St.",
        Street2 = null,
        City = "Alt-ville",
        PostalCode = "45678",
        State = "GA",
    };

    private static Address IncompleteAddress => new()
    {
        Street = string.Empty,
        Street2 = null,
        City = "Ghost-town",
        PostalCode = string.Empty,
        State = string.Empty,
    };
}
