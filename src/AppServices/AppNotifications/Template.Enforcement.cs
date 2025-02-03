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

    public static readonly Template EnforcementCommentAdded = new()
    {
        Subject = "New Comment on Enforcement",
        TextBody =
            """
            Enforcement ID {0} has a new comment by {2}:

            {1}

            """,
        HtmlBody = "<p>Enforcement ID {0} has a new comment by {2}:</p>" +
                   "<blockquote style='white-space:pre-line'>{1}</blockquote>",
    };

    public static readonly Template EnforcementUpdated = new()
    {
        Subject = "Enforcement Updated",
        TextBody = "Enforcement ID {0} has been updated.",
        HtmlBody = "<p>Enforcement ID {0} has been updated.</p>",
    };
}
