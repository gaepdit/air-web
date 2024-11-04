namespace AirWeb.Domain.DataExchange;

public static class Identifiers
{
    public static string GetEpaFacilityId(this FacilityId facilityId) => $"GA00000013{facilityId.Id}";

    public static string GetEpaActionId(this short? actionNumber, Facility facility) =>
        actionNumber is null ? string.Empty : $"GA000A000013{facility.Id.Id}{actionNumber:D5}";
}
