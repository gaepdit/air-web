using AirWeb.Domain.EmailLog;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalEmailLogRepository : IEmailLogRepository
{
    // No work is needed here since, from an app perspective, the Email Log Repository is write-only.
    // Therefore, the in-memory repository will never be read.
    public Task InsertAsync(EmailLog emailLog, CancellationToken token = default) => Task.CompletedTask;

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;

    #endregion
}
