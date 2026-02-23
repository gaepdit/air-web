using AirWeb.Domain.Core.Entities;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.CommonRepositories;

public sealed class EmailLogRepository(AppDbContext context) : IEmailLogRepository
{
    public async Task InsertAsync(EmailLog emailLog, CancellationToken token = default)
    {
        await context.EmailLogs.AddAsync(emailLog, token).ConfigureAwait(false);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => context.Dispose();
    public ValueTask DisposeAsync() => context.DisposeAsync();

    #endregion
}
