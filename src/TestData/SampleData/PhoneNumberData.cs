using AirWeb.Domain.Sbeap.ValueObjects;

namespace AirWeb.TestData.SampleData;

public static class PhoneNumberData
{
    public static PhoneNumber Phone1 => new()
    {
        Id = 1,
        Number = "404-555-1212",
        Type = PhoneType.Work,
    };

    public static PhoneNumber Phone2 => new()
    {
        Id = 2,
        Number = "678-555-1212",
        Type = PhoneType.WorkCell,
    };

    public static PhoneNumber Phone3 => new()
    {
        Id = 3,
        Number = "770-555-1212",
        Type = PhoneType.Unknown,
    };

    public static PhoneNumber Phone4 => new()
    {
        Id = 4,
        Number = "404-555-1212",
        Type = PhoneType.Work,
    };

    public static PhoneNumber Phone5 => new()
    {
        Id = 5,
        Number = "678-555-1212",
        Type = PhoneType.WorkCell,
    };

    public static PhoneNumber Phone6 => new()
    {
        Id = 6,
        Number = "770-555-1212",
        Type = PhoneType.Unknown,
    };
}
