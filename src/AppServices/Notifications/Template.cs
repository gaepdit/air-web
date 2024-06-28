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
        HtmlBody = "<p>Work Entry ID {0} has been added.",
    };

    public static readonly Template EntryUpdated = new()
    {
        Subject = "Work Entry Updated",
        TextBody = "Work Entry ID {0} has been updated.",
        HtmlBody = "<p>Work Entry ID {0} has been updated.",
    };

    public static readonly Template EntryCommentAdded = new()
    {
        Subject = "New Comment on Work Entry",
        TextBody = "Work Entry ID {0} has a new comment.",
        HtmlBody = "<p>Work Entry ID {0} has a new comment.",
    };

    public static readonly Template EntryReopened = new()
    {
        Subject = "Work Entry Reopened",
        TextBody = "Work Entry ID {0} has been reopened.",
        HtmlBody = "<p>Work Entry ID {0} has been reopened.",
    };

    // FCE email templates
    public static readonly Template FceCreated = new()
    {
        Subject = "New FCE Added",
        TextBody = "FCE ID {0} has been added.",
        HtmlBody = "<p>FCE ID {0} has been added.",
    };

    public static readonly Template FceUpdated = new()
    {
        Subject = "FCE Updated",
        TextBody = "FCE ID {0} has been updated.",
        HtmlBody = "<p>FCE ID {0} has been updated.",
    };

    public static readonly Template FceCommentAdded = new()
    {
        Subject = "New Comment on FCE",
        TextBody = "FCE ID {0} has a new comment.",
        HtmlBody = "<p>FCE ID {0} has a new comment.",
    };

    public static readonly Template FceReopened = new()
    {
        Subject = "FCE Reopened",
        TextBody = "FCE ID {0} has been reopened.",
        HtmlBody = "<p>FCE ID {0} has been reopened.",
    };

    // Email signatures
    public const string TextSignature =
        """


        --
        This is an automatically generated email.
        """;

    public const string HtmlSignature = "<hr><p>This is an automatically generated email.</p>";
}
