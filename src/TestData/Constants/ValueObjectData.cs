using AirWeb.Domain.ValueObjects;

namespace AirWeb.TestData.Constants;

public static class ValueObjectData
{
    public static Address CompleteAddress => new()
    {
        Street = "123 Main St.",
        Street2 = "Box 456",
        City = "Town-ville",
        PostalCode = "98765-1234",
        State = "Georgia",
    };

    public static Address LessCompleteAddress => new()
    {
        Street = "456 Second St.",
        Street2 = null,
        City = "Alt-ville",
        PostalCode = "98765",
        State = "GA",
    };
}
