namespace AirWeb.AppServices.Notifications;

public interface INotificationService
{
    Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail, CancellationToken token,
        params object?[] values);
}
