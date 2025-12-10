using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.TestData.Lookups;
using IaipDataService.Facilities;
using IaipDataService.TestData;

namespace AirWeb.TestData.SampleData;

public static class DomainData
{
    public static Facility GetRandomFacility() => FacilityData.GetRandomFacility();

    public static NotificationType GetRandomNotificationType() =>
        NotificationTypeData.GetData[Random.Shared.Next(NotificationTypeData.GetData.Count)];
}
