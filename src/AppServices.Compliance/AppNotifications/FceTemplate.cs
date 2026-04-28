using AirWeb.AppServices.Core.AppNotifications;

namespace AirWeb.AppServices.Compliance.AppNotifications;

public class FceTemplate : Template
{
    // FCE email templates
    private const string FceCreatedBody = "FCE ID {0} has been added for Facility {1} for {2} by {3}.";

    public static readonly FceTemplate FceCreated = new()
    {
        Subject = "New FCE Added",
        TextBody = FceCreatedBody,
        HtmlBody = $"<p>{FceCreatedBody}</p>",
    };

    private const string FceUpdatedBody = "FCE ID {0} for Facility {1} for {2} has been updated by {3}.";

    public static readonly FceTemplate FceUpdated = new()
    {
        Subject = "FCE Updated",
        TextBody = FceUpdatedBody,
        HtmlBody = $"<p>{FceUpdatedBody}</p>",
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

    private const string FceDeletedBody = "FCE ID {0} for Facility {1} for {2} has been deleted by {3}.";

    public static readonly FceTemplate FceDeleted = new()
    {
        Subject = "FCE Deleted",
        TextBody = FceDeletedBody,
        HtmlBody = $"<p>{FceDeletedBody}</p>",
    };

    private const string FceRestoredBody = "FCE ID {0} for Facility {1} for {2} has been restored by {3}.";

    public static readonly FceTemplate FceRestored = new()
    {
        Subject = "FCE Restored",
        TextBody = FceRestoredBody,
        HtmlBody = $"<p>{FceRestoredBody}</p>",
    };
}
