namespace AirWeb.AppServices.Notifications;

public class Template
{
    // Email template properties
    public required string Subject { get; init; }
    public required string TextBody { get; init; }
    public required string HtmlBody { get; init; }

    // Work Entry email templates
    public static readonly Template EntryCreated = new()
    {
        Subject = "New Work Entry Added",
        TextBody = "Work Entry ID {0} has been added.",
        HtmlBody = "<p>Work Entry ID {0} has been added.</p>",
    };

    public static readonly Template EntryUpdated = new()
    {
        Subject = "Work Entry Updated",
        TextBody = "Work Entry ID {0} has been updated.",
        HtmlBody = "<p>Work Entry ID {0} has been updated.</p>",
    };

    public static readonly Template EntryCommentAdded = new()
    {
        Subject = "New Comment on Work Entry",
        TextBody =
            """
            Work Entry ID {0} has a new comment by {2}:

            {1}

            """,
        HtmlBody = "<p>Work Entry ID {0} has a new comment by {2}:</p>" +
                   "<blockquote style='white-space:pre-line'>{1}</blockquote>",
    };

    public static readonly Template EntryClosed = new()
    {
        Subject = "Work Entry Closed",
        TextBody = "Work Entry ID {0} has been closed.",
        HtmlBody = "<p>Work Entry ID {0} has been closed.</p>",
    };

    public static readonly Template EntryReopened = new()
    {
        Subject = "Work Entry Reopened",
        TextBody = "Work Entry ID {0} has been reopened.",
        HtmlBody = "<p>Work Entry ID {0} has been reopened.</p>",
    };

    public static readonly Template EntryDeleted = new()
    {
        Subject = "Work Entry Deleted",
        TextBody = "Work Entry ID {0} has been deleted.",
        HtmlBody = "<p>Work Entry ID {0} has been deleted.</p>",
    };

    public static readonly Template EntryRestored = new()
    {
        Subject = "Work Entry Restored",
        TextBody = "Work Entry ID {0} has been restored.",
        HtmlBody = "<p>Work Entry ID {0} has been restored.</p>",
    };

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

    // Email signatures
    public const string TextSignature =
        """


        --
        This is an automatically generated email.
        """;

    public const string HtmlSignature = "<hr><p>This is an automatically generated email.</p>";
}
