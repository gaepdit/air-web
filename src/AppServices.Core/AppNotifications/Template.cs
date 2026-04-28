namespace AirWeb.AppServices.Core.AppNotifications;

public abstract class Template
{
    // Email template properties
    public required string Subject { get; init; }
    public required string TextBody { get; init; }
    public string? HtmlBody { get; protected init; }

    // Email signatures
    public const string TextSignature =
        """


        --
        This is an automatically generated email.
        """;

    public const string HtmlSignature = "<hr><p>This is an automatically generated email.</p>";
}
