namespace AirWeb.AppServices.AppNotifications;

public record AppNotificationResult
{
    private AppNotificationResult() { }

    public bool Success { get; private init; }
    public bool Attempted { get; private init; }
    public string FailureMessage { get; private init; } = string.Empty;

    public static AppNotificationResult SuccessResult() => new()
        { Success = true };

    public static AppNotificationResult NotAttemptedResult() => new()
        { FailureMessage = "No notification email was sent." };

    public static AppNotificationResult FailureResult(string failureMessage) => new()
        { Attempted = true, FailureMessage = failureMessage };
}
