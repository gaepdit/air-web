namespace AirWeb.AppServices.AppNotifications;

public partial class Template
{
    // FCE email templates
    public static readonly Template FceCreated = new()
    {
        Subject = "New FCE Added",
        TextBody = "FCE ID {0} has been added.",
        HtmlBody = "<p>FCE ID {0} has been added.</p>",
    };

    public static readonly Template FceUpdated = new()
    {
        Subject = "FCE Updated",
        TextBody = "FCE ID {0} has been updated.",
        HtmlBody = "<p>FCE ID {0} has been updated.</p>",
    };

    public static readonly Template FceCommentAdded = new()
    {
        Subject = "New Comment on FCE",
        TextBody =
            """
            FCE ID {0} has a new comment by {2}:

            {1}

            """,
        HtmlBody = "<p>FCE ID {0} has a new comment by {2}:</p>" +
                   "<blockquote style='white-space:pre-line'>{1}</blockquote>",
    };

    public static readonly Template FceDeleted = new()
    {
        Subject = "FCE Deleted",
        TextBody = "FCE ID {0} has been deleted.",
        HtmlBody = "<p>FCE ID {0} has been deleted.</p>",
    };

    public static readonly Template FceRestored = new()
    {
        Subject = "FCE Restored",
        TextBody = "FCE ID {0} has been restored.",
        HtmlBody = "<p>FCE ID {0} has been restored.</p>",
    };
}
