using AirWeb.Domain.Entities.NotificationTypes;

namespace AirWeb.TestData.Entities;

internal static class NotificationTypeData
{
    private static List<NotificationType> NotificationTypeSeedItems =>
    [
        new NotificationType(new Guid("20000000-0000-0000-0000-000000000021"), "Other"),
        new NotificationType(new Guid("20000000-0000-0000-0000-000000000022"), "Startup"),
        new NotificationType(new Guid("20000000-0000-0000-0000-000000000023"), "Response Letter"),
        new NotificationType(new Guid("20000000-0000-0000-0000-000000000024"), "Malfunction"),
        new NotificationType(new Guid("20000000-0000-0000-0000-000000000025"), "Deviation"),
        new NotificationType(new Guid("20000000-0000-0000-0000-000000000026"), "Permit Revocation") { Active = false },
    ];

    private static List<NotificationType>? _notificationTypes;

    public static List<NotificationType> GetData
    {
        get
        {
            if (_notificationTypes is not null) return _notificationTypes;
            _notificationTypes = NotificationTypeSeedItems;
            return _notificationTypes;
        }
    }

    public static void ClearData() => _notificationTypes = null;
}
