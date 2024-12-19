using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.AppNotifications;

public static class AppNotificationExtensions
{
    public static Task<AppNotificationResult> SendNotificationAsync(this IAppNotificationService service,
        Template template, ApplicationUser? recipient, CancellationToken token, params object?[] values)
    {
        if (recipient is null)
            return Task.FromResult(AppNotificationResult.NotAttemptedResult());
        if (!recipient.Active)
            return Task.FromResult(AppNotificationResult.FailureResult("The recipient is not an active user."));
        if (recipient.Email is null)
            return Task.FromResult(AppNotificationResult.FailureResult("The recipient cannot be emailed."));

        return service.SendNotificationAsync(template, recipient.Email, token, values);
    }
}
