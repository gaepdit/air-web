using AirWeb.Domain.Data;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.ExternalEntities;
using AirWeb.TestData.NamedEntities;

namespace AirWeb.TestData.SampleData;

public static class DomainData
{
    public static string GetRandomCounty() =>
        Data.Counties[new Random().Next(Data.Counties.Count)];

    public static Facility GetRandomFacility() =>
        FacilityData.GetData[new Random().Next(FacilityData.GetData.Count)];

    public static NotificationType GetRandomNotificationType() =>
        NotificationTypeData.GetData[new Random().Next(NotificationTypeData.GetData.Count)];
}
