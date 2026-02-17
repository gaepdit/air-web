namespace AirWeb.Domain.Core.Entities;

public interface IEmailLogRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Saves a copy of an <see cref="EmailLog"/> to the configured repository.
    /// </summary>
    /// <param name="emailLog">The email message to log.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertAsync(EmailLog emailLog, CancellationToken token = default);
}

public record EmailLog
{
    [Key]
    public required Guid Id { get; init; }

    public DateTimeOffset? CreatedAt { get; init; }

    [StringLength(300)]
    public required string Sender { get; init; }

    [StringLength(200)]
    public required string Subject { get; init; }

    [StringLength(2000)]
    public required string Recipients { get; init; }

    [StringLength(2000)]
    public string? CopyRecipients { get; init; }

    [StringLength(15_000)]
    public string? TextBody { get; init; }

    [StringLength(20_000)]
    public string? HtmlBody { get; init; }
}
