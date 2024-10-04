using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.NamedEntities;
using IaipDataService.Facilities;

namespace AirWeb.TestData.SampleData;

public static class DomainData
{
    public static Facility GetRandomFacility() => FacilityData.GetRandomFacility();

    public static NotificationType GetRandomNotificationType() =>
        NotificationTypeData.GetData[new Random().Next(NotificationTypeData.GetData.Count)];
}
