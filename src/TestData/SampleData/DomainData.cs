using AirWeb.Domain.Data;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.NamedEntities;
using IaipDataService.Facilities;

namespace AirWeb.TestData.SampleData;

public static class DomainData
{
    public static string GetRandomCounty() =>
        Data.Counties[new Random().Next(Data.Counties.Count)];

    public static Facility GetRandomFacility() => FacilityTestData.GetRandomFacility();

    public static NotificationType GetRandomNotificationType() =>
        NotificationTypeData.GetData[new Random().Next(NotificationTypeData.GetData.Count)];
}
