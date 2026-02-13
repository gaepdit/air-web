using System.Diagnostics.CodeAnalysis;

namespace AirWeb.AppServices.Core.AppNotifications;

public record AppNotificationResult
{
    private static readonly AppNotificationResult SucceededResult = new() { Succeeded = true };

    private static readonly AppNotificationResult NotAttemptedResult =
        new() { FailureReason = "No notification email was sent." };

    private AppNotificationResult() { }

    [MemberNotNullWhen(false, nameof(FailureReason))]
    public bool Succeeded { get; private init; }

    public string? FailureReason { get; private init; }

    public static AppNotificationResult Success() => SucceededResult;
    public static AppNotificationResult NotAttempted() => NotAttemptedResult;
    public static AppNotificationResult Failed(string failureMessage) => new() { FailureReason = failureMessage };
}
