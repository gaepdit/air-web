﻿using AirWeb.AppServices.ErrorLogging;
using GaEpd.EmailService;
using GaEpd.EmailService.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AirWeb.AppServices.Notifications;

public class NotificationService(
    IEmailService emailService,
    IEmailLogRepository repository,
    IHostEnvironment environment,
    IConfiguration configuration,
    IErrorLogger errorLogger) : INotificationService
{
    private const string FailurePrefix = "Notification email not sent:";

    public async Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail,
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
            return NotificationResult.FailureResult($"{FailurePrefix} A recipient could not be determined.");

        Message message;
        try
        {
            message = Message.Create(subject, recipientEmail, settings.DefaultSender, textBody, htmlBody);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when generating the email.");
        }

        if (settings.SaveEmail) await repository.InsertAsync(EmailLog.Create(message), token).ConfigureAwait(false);

        if (settings is { EnableEmail: false, EnableEmailAuditing: false })
        {
            return NotificationResult.FailureResult($"{FailurePrefix} Emailing is not enabled on the server.");
        }

        try
        {
            await emailService.SendEmailAsync(message, settings, token).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when sending the email.");
        }

        return NotificationResult.SuccessResult();
    }
}
