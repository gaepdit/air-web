using MyApp.Domain.Entities.WorkEntries;

namespace MyApp.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, WorkEntry workEntry,
        CancellationToken token = default);
}
