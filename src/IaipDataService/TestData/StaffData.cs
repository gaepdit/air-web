using IaipDataService.Structs;

namespace IaipDataService.TestData;

public static class StaffData
{
    public static Staff GetRandomStaff() => GetData[Random.Shared.Next(GetData.Count)];

    private static IEnumerable<Staff> StaffSeedItems =>
    [
        new()
        {
            Id = 1,
            EmailAddress = "one@example.net",
            Name = new PersonName("Adélie", "Penguin"),
        },
        new()
        {
            Id = 2,
            EmailAddress = "two@example.net",
            Name = new PersonName("Bactrian", "Camel"),
        },
        new()
        {
            Id = 3,
            EmailAddress = "three@example.net",
            Name = new PersonName("Clouded", "Leopard", Suffix: "Jr."),
        },
        new()
        {
            Id = 4,
            EmailAddress = "four@example.net",
            Name = new PersonName("Dugong", "Sirenia", "Ms."),
        },
        new()
        {
            Id = 5,
            EmailAddress = "five@example.net",
            Name = new PersonName("Elephant", "Seal"),
        },
        new()
        {
            Id = 6,
            EmailAddress = "six@example.net",
            Name = new PersonName("Fennec", "Fox"),
        },
    ];

    private static List<Staff>? _staff;

    public static List<Staff> GetData
    {
        get
        {
            if (_staff is not null) return _staff;
            _staff = StaffSeedItems.ToList();
            return _staff;
        }
    }
}
