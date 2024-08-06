﻿using AirWeb.EfRepository.DbContext;
using GaEpd.EmailService.Repository;

namespace AirWeb.EfRepository.Repositories;

public sealed class EmailLogRepository(AppDbContext context) : IEmailLogRepository
{
    public async Task InsertAsync(EmailLog emailLog, CancellationToken token = default)
    {
        await context.EmailLogs.AddAsync(emailLog, token).ConfigureAwait(false);
        await context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public void Dispose() => context.Dispose();
    public ValueTask DisposeAsync() => context.DisposeAsync();
}
