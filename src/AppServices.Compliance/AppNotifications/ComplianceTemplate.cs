using AirWeb.AppServices.Core.AppNotifications;

namespace AirWeb.AppServices.Compliance.AppNotifications;

public class ComplianceTemplate : Template
{
    // Compliance Work email templates

    private const string WorkCreatedBody = "Compliance Work ID {0} has been added for Facility {1} by {2}.";

    public static readonly ComplianceTemplate WorkCreated = new()
    {
        Subject = "New Compliance Work Added",
        TextBody = WorkCreatedBody,
        HtmlBody = $"<p>{WorkCreatedBody}</p>",
    };

    private const string WorkUpdatedBody = "Compliance Work ID {0} has been updated by {1}.";

    public static readonly ComplianceTemplate WorkUpdated = new()
    {
        Subject = "Compliance Work Updated",
        TextBody = WorkUpdatedBody,
        HtmlBody = $"<p>{WorkUpdatedBody}</p>",
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

    private const string WorkClosedBody = "Compliance Work ID {0} has been closed by {1}.";

    public static readonly ComplianceTemplate WorkClosed = new()
    {
        Subject = "Compliance Work Closed",
        TextBody = WorkClosedBody,
        HtmlBody = $"<p>{WorkClosedBody}</p>",
    };

    private const string WorkReopenedBody = "Compliance Work ID {0} has been reopened by {1}.";

    public static readonly ComplianceTemplate WorkReopened = new()
    {
        Subject = "Compliance Work Reopened",
        TextBody = WorkReopenedBody,
        HtmlBody = $"<p>{WorkReopenedBody}</p>",
    };

    private const string WorkDeletedBody = "Compliance Work ID {0} has been deleted by {1}.";

    public static readonly ComplianceTemplate WorkDeleted = new()
    {
        Subject = "Compliance Work Deleted",
        TextBody = WorkDeletedBody,
        HtmlBody = $"<p>{WorkDeletedBody}</p>",
    };

    private const string WorkRestoredBody = "Compliance Work ID {0} has been restored {1}.";

    public static readonly ComplianceTemplate WorkRestored = new()
    {
        Subject = "Compliance Work Restored",
        TextBody = WorkRestoredBody,
        HtmlBody = $"<p>{WorkRestoredBody}</p>",
    };
}
