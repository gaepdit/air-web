using AirWeb.Domain.EmailLog;
using AirWeb.Domain.Identity;
using GaEpd.AppLibrary.Extensions;
using GaEpd.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AirWeb.AppServices.AppNotifications;

public interface IAppNotificationService
{
    Task<AppNotificationResult> SendNotificationAsync(Template template, string recipientEmail, CancellationToken token,
        params object?[] values);
}

public class AppNotificationService(
    IEmailService emailService,
    IEmailLogRepository emailLogRepository,
    IHostEnvironment environment,
    IConfiguration configuration,
    ILogger<AppNotificationService> logger) : IAppNotificationService
{
    private const string FailurePrefix = "Notification email not sent:";
    internal static readonly EventId AppNotificationServiceFailure = new(2501, nameof(AppNotificationServiceFailure));

    public async Task<AppNotificationResult> SendNotificationAsync(Template template, string recipientEmail,
        CancellationToken token, params object?[] values)
    {
        var subjectPrefix = environment.EnvironmentName switch
        {
            "Development" => "[DEV] ",
            "Staging" => "[UAT] ",
            _ => "",
        };

        var subject = $"{subjectPrefix} {template.Subject}";
        var textBody = string.Format(template.TextBody + Template.TextSignature, values);
        var htmlBody = string.Format(template.HtmlBody + Template.HtmlSignature, values);

        var settings = new EmailServiceSettings();
        configuration.GetSection(nameof(EmailServiceSettings)).Bind(settings);

        if (string.IsNullOrEmpty(recipientEmail))
            return AppNotificationResult.Failed($"{FailurePrefix} A recipient could not be determined.");

        Message message;
        try
        {
            message = Message.Create(subject, recipientEmail, textBody, htmlBody);
        }
        catch (Exception e)
        {
            logger.LogError(AppNotificationServiceFailure, e, "Failure generating email message with subject {Subject}",
                subject);
            return AppNotificationResult.Failed($"{FailurePrefix} An error occurred when generating the email.");
        }

        await emailLogRepository.InsertAsync(Create(message), token).ConfigureAwait(false);

        if (settings is { EnableEmail: false, EnableEmailAuditing: false })
        {
            return AppNotificationResult.Failed($"{FailurePrefix} Emailing is not enabled on the server.");
        }

        try
        {
            _ = emailService.SendEmailAsync(message, token);
        }
        catch (Exception e)
        {
            logger.LogError(AppNotificationServiceFailure, e, "Failure sending email message with subject {Subject}",
                subject);
            return AppNotificationResult.Failed($"{FailurePrefix} An error occurred when sending the email.");
        }

        return AppNotificationResult.Success();
    }

    private static EmailLog Create(Message message) => new()
    {
        Id = Guid.NewGuid(),
        Sender = StringExtensions.ConcatWithSeparator([message.SenderName, $"<{message.SenderEmail}>"])
            .Truncate(300),
        Subject = message.Subject.Truncate(200),
        Recipients = message.Recipients.ConcatWithSeparator(",").Truncate(2000),
        CopyRecipients = message.CopyRecipients.ConcatWithSeparator(",").Truncate(2000),
        TextBody = message.TextBody.Truncate(15_000),
        HtmlBody = message.HtmlBody.Truncate(20_000),
        CreatedAt = DateTimeOffset.Now,
    };
}

public static class AppNotificationExtensions
{
    public static async Task<AppNotificationResult> SendNotificationAsync(this IAppNotificationService service,
        Template template, ApplicationUser? recipient, CancellationToken token, params object?[] values)
    {
        if (recipient is null)
            return AppNotificationResult.NotAttempted();
        if (!recipient.Active)
            return AppNotificationResult.Failed("The recipient is not an active user.");
        if (recipient.Email is null)
            return AppNotificationResult.Failed("The recipient has no email address.");

        return await service.SendNotificationAsync(template, recipient.Email, token, values).ConfigureAwait(false);
    }
}
