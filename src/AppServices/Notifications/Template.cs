namespace AirWeb.AppServices.Notifications;

public class Template
{
    // Email template properties
    public required string Subject { get; init; }
    public required string TextBody { get; init; }
    public required string HtmlBody { get; init; }

    // Email templates
    public static readonly Template NewEntry = new()
    {
        Subject = "New Work Entry",
        TextBody = "Work Entry ID {0} has been created.",
        HtmlBody = "<p>Work Entry ID {0} has been created.",
    };

    public static readonly Template UpdatedEntry = new()
    {
        Subject = "Updated Work Entry",
        TextBody = "Work Entry ID {0} has been updated.",
        HtmlBody = "<p>Work Entry ID {0} has been updated.",
    };

    public static readonly Template CommentAdded = new()
    {
        Subject = "New Comment on Work Entry",
        TextBody = "Work Entry ID {0} has a new comment.",
        HtmlBody = "<p>Work Entry ID {0} has a new comment.",
    };

    public static readonly Template Reopened = new()
    {
        Subject = "Work Entry Reopened",
        TextBody = "Work Entry ID {0} has been reopened.",
        HtmlBody = "<p>Work Entry ID {0} has been reopened.",
    };

    // Email signatures
    public const string TextSignature =
        """


        --
        This is an automatically generated email.
        """;

    public const string HtmlSignature = "<hr><p>This is an automatically generated email.</p>";
}
