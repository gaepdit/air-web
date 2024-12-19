using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.NamedEntities;
using IaipDataService.Facilities;
using IaipDataService.TestData;

namespace AirWeb.TestData.SampleData;

public static class DomainData
{
    public static Facility GetRandomFacility() => FacilityData.GetRandomFacility();

    public static NotificationType GetRandomNotificationType() =>
        NotificationTypeData.GetData[Random.Shared.Next(NotificationTypeData.GetData.Count)];
}
