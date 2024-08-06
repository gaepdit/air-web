namespace AirWeb.AppServices.AppNotifications;

public interface IAppNotificationService
{
    Task<AppNotificationResult> SendNotificationAsync(Template template, string recipientEmail, CancellationToken token,
        params object?[] values);
}
