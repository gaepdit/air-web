namespace AirWeb.Domain.EnforcementEntities;

public static class Helpers
{
    public static string GetEpaIdFromActionNumber(this Facility facility, short? actionNumber) =>
        actionNumber is null ? string.Empty : $"GA000A000013{facility.Id.Id}{actionNumber:D5}";
}
