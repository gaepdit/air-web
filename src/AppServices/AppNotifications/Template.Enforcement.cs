namespace AirWeb.AppServices.AppNotifications;

public partial class Template
{
    // Enforcement email templates
    public static readonly Template EnforcementCreated = new()
    {
        Subject = "Enforcement Case Started",
        TextBody = "Enforcement Case ID {0} has been started.",
        HtmlBody = "<p>Enforcement Case ID {0} has been started.</p>",
    };

    public static readonly Template EnforcementUpdated = new()
    {
        Subject = "Enforcement Case Updated",
        TextBody = "Enforcement Case ID {0} has been updated.",
        HtmlBody = "<p>Enforcement Case ID {0} has been updated.</p>",
    };

    public static readonly Template EnforcementCommentAdded = new()
    {
        Subject = "New Comment on Enforcement Case",
        TextBody = "A comment has been added to Enforcement Case ID {0}.",
        HtmlBody = "<p>A comment has been added to Enforcement Case ID {0}.</p>",
    };

    public static readonly Template EnforcementClosed = new()
    {
        Subject = "Enforcement Case Closed",
        TextBody = "Enforcement Case ID {0} has been closed.",
        HtmlBody = "<p>Enforcement Case ID {0} has been closed.</p>",
    };

    public static readonly Template EnforcementReopened = new()
    {
        Subject = "Enforcement Case Reopened",
        TextBody = "Enforcement Case ID {0} has been reopened.",
        HtmlBody = "<p>Enforcement Case ID {0} has been reopened.</p>",
    };

    public static readonly Template EnforcementDeleted = new()
    {
        Subject = "Enforcement Case Deleted",
        TextBody = "Enforcement Case ID {0} has been deleted.",
        HtmlBody = "<p>Enforcement Case ID {0} has been deleted.</p>",
    };

    public static readonly Template EnforcementRestored = new()
    {
        Subject = "Enforcement Case Restored",
        TextBody = "Enforcement Case ID {0} has been restored.",
        HtmlBody = "<p>Enforcement Case ID {0} has been restored.</p>",
    };

    public static readonly Template EnforcementActionAdded = new()
    {
        Subject = "Enforcement Case Updated",
        TextBody = "An Enforcement Action has been added to Enforcement Case ID {0}.",
        HtmlBody = "<p>An Enforcement Action has been added to Enforcement Case ID {0}.</p>",
    };

    public static readonly Template EnforcementActionDeleted = new()
    {
        Subject = "Enforcement Case Updated",
        TextBody = "An Enforcement Action was deleted from Enforcement Case ID {0}.",
        HtmlBody = "<p>An Enforcement Action was deleted from Enforcement Case ID {0}.</p>",
    };

    public static readonly Template EnforcementActionReviewRequested = new()
    {
        Subject = "Enforcement Action Review Requested",
        TextBody = "Your review has been requested for an Enforcement Action on Case ID {0}.",
        HtmlBody = "<p>Your review has been requested for an Enforcement Action on Case ID {0}.</p>",
    };

    public static readonly Template EnforcementActionReviewCompleted = new()
    {
        Subject = "Enforcement Action Review Completed",
        TextBody = "An Enforcement Action review has been completed on Case ID {0}.",
        HtmlBody = "<p>An Enforcement Action review has been completed on Case ID {0}.</p>",
    };
}
