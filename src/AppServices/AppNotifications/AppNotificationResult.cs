namespace AirWeb.AppServices.AppNotifications;

public record AppNotificationResult
{
    private AppNotificationResult() => Success = true;
    private AppNotificationResult(string failureMessage) => FailureMessage = failureMessage;

    public bool Attempted { get; init; } = true;
    public bool Success { get; }
    public string FailureMessage { get; } = string.Empty;

    public static AppNotificationResult SuccessResult() => new();
    public static AppNotificationResult NotAttemptedResult() => new() { Attempted = false };
    public static AppNotificationResult FailureResult(string failureMessage) => new(failureMessage);
}
