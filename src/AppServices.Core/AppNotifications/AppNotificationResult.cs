namespace AirWeb.AppServices.Core.AppNotifications;

public record AppNotificationResult
{
    private static readonly AppNotificationResult SucceededResult = new() { Succeeded = true, Attempted = true };

    private static readonly AppNotificationResult NotAttemptedResult = new();

    private AppNotificationResult() { }

    public bool Succeeded { get; private init; }
    public bool Attempted { get; private init; }
    public string? FailureReason { get; private init; }

    public static AppNotificationResult Success() => SucceededResult;
    public static AppNotificationResult NotAttempted() => NotAttemptedResult;

    public static AppNotificationResult Failed(string failureMessage) =>
        new() { Attempted = true, FailureReason = failureMessage };
}
