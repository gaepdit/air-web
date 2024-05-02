using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, WorkEntry workEntry,
        CancellationToken token = default);
}
