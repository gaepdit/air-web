using AirWeb.AppServices.Core.AppNotifications;

namespace AirWeb.AppServices.Compliance.AppNotifications;

public class FceTemplate : Template
{
    // FCE email templates
    public static readonly FceTemplate FceCreated = new()
    {
        Subject = "New FCE Added",
        TextBody = "FCE ID {0} has been added.",
        HtmlBody = "<p>FCE ID {0} has been added.</p>",
    };

    public static readonly FceTemplate FceUpdated = new()
    {
        Subject = "FCE Updated",
        TextBody = "FCE ID {0} has been updated.",
        HtmlBody = "<p>FCE ID {0} has been updated.</p>",
    };

    public static readonly FceTemplate FceCommentAdded = new()
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

    public static readonly FceTemplate FceDeleted = new()
    {
        Subject = "FCE Deleted",
        TextBody = "FCE ID {0} has been deleted.",
        HtmlBody = "<p>FCE ID {0} has been deleted.</p>",
    };

    public static readonly FceTemplate FceRestored = new()
    {
        Subject = "FCE Restored",
        TextBody = "FCE ID {0} has been restored.",
        HtmlBody = "<p>FCE ID {0} has been restored.</p>",
    };
}
