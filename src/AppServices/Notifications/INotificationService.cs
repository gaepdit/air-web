using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, BaseWorkEntry baseWorkEntry,
        CancellationToken token = default);
}
