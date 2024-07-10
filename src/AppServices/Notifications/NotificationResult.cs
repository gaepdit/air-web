namespace AirWeb.AppServices.Notifications;

public record NotificationResult
{
    private NotificationResult() => Success = true;
    private NotificationResult(string failureMessage) => FailureMessage = failureMessage;

    public bool Attempted { get; init; } = true;
    public bool Success { get; }
    public string FailureMessage { get; } = string.Empty;

    public static NotificationResult SuccessResult() => new();
    public static NotificationResult NotAttemptedResult() => new() { Attempted = false };
    public static NotificationResult FailureResult(string failureMessage) => new(failureMessage);
}
