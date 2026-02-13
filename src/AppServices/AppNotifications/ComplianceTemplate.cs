using AirWeb.AppServices.Core.AppNotifications;

namespace AirWeb.AppServices.AppNotifications;

public class ComplianceTemplate : Template
{
    // Compliance Work email templates
    public static readonly ComplianceTemplate WorkCreated = new()
    {
        Subject = "New Compliance Work Added",
        TextBody = "Compliance Work ID {0} has been added.",
        HtmlBody = "<p>Compliance Work ID {0} has been added.</p>",
    };

    public static readonly ComplianceTemplate WorkUpdated = new()
    {
        Subject = "Compliance Work Updated",
        TextBody = "Compliance Work ID {0} has been updated.",
        HtmlBody = "<p>Compliance Work ID {0} has been updated.</p>",
    };

    public static readonly ComplianceTemplate WorkCommentAdded = new()
    {
        Subject = "New Comment on Compliance Work",
        TextBody =
            """
            Compliance Work ID {0} has a new comment by {2}:

            {1}

            """,
        HtmlBody = "<p>Compliance Work ID {0} has a new comment by {2}:</p>" +
                   "<blockquote style='white-space:pre-line'>{1}</blockquote>",
    };

    public static readonly ComplianceTemplate WorkClosed = new()
    {
        Subject = "Compliance Work Closed",
        TextBody = "Compliance Work ID {0} has been closed.",
        HtmlBody = "<p>Compliance Work ID {0} has been closed.</p>",
    };

    public static readonly ComplianceTemplate WorkReopened = new()
    {
        Subject = "Compliance Work Reopened",
        TextBody = "Compliance Work ID {0} has been reopened.",
        HtmlBody = "<p>Compliance Work ID {0} has been reopened.</p>",
    };

    public static readonly ComplianceTemplate WorkDeleted = new()
    {
        Subject = "Compliance Work Deleted",
        TextBody = "Compliance Work ID {0} has been deleted.",
        HtmlBody = "<p>Compliance Work ID {0} has been deleted.</p>",
    };

    public static readonly ComplianceTemplate WorkRestored = new()
    {
        Subject = "Compliance Work Restored",
        TextBody = "Compliance Work ID {0} has been restored.",
        HtmlBody = "<p>Compliance Work ID {0} has been restored.</p>",
    };
}
