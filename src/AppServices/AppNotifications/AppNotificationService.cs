using AirWeb.AppServices.ErrorLogging;
using AirWeb.Domain.EmailLog;
using GaEpd.AppLibrary.Extensions;
using GaEpd.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AirWeb.AppServices.AppNotifications;

public class AppNotificationService(
    IEmailService emailService,
    IEmailLogRepository emailLogRepository,
    IHostEnvironment environment,
    IConfiguration configuration,
    IErrorLogger errorLogger) : IAppNotificationService
{
    private const string FailurePrefix = "Notification email not sent:";

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
            return AppNotificationResult.FailureResult($"{FailurePrefix} A recipient could not be determined.");

        Message message;
        try
        {
            message = Message.Create(subject, recipientEmail, textBody, htmlBody);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return AppNotificationResult.FailureResult($"{FailurePrefix} An error occurred when generating the email.");
        }

        await emailLogRepository.InsertAsync(Create(message), token).ConfigureAwait(false);

        if (settings is { EnableEmail: false, EnableEmailAuditing: false })
        {
            return AppNotificationResult.FailureResult($"{FailurePrefix} Emailing is not enabled on the server.");
        }

        try
        {
            _ = emailService.SendEmailAsync(message, token);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return AppNotificationResult.FailureResult($"{FailurePrefix} An error occurred when sending the email.");
        }

        return AppNotificationResult.SuccessResult();
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
