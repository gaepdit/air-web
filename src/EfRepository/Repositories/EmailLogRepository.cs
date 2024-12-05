using AirWeb.EfRepository.DbContext;
using GaEpd.EmailService;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.Extensions.Configuration;

namespace AirWeb.EfRepository.Repositories;

public sealed class EmailLogRepository(AppDbContext context, IConfiguration configuration) : IEmailLogRepository
{
    public async Task InsertAsync(Message message, CancellationToken token = default)
    {
        var settings = new EmailServiceSettings();
        configuration.GetSection(nameof(EmailServiceSettings)).Bind(settings);
        if (!settings.EnableEmailLog) return;

        var emailLog = EmailLog.Create(message);
        await context.EmailLogs.AddAsync(emailLog, token).ConfigureAwait(false);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => context.Dispose();
    public ValueTask DisposeAsync() => context.DisposeAsync();

    #endregion
}
