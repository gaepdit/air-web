using System.Text;

namespace AirWeb.Domain.Core.Entities;

public record SicCode : IEntity<string>
{
    [Key, MaxLength(4)]
    public required string Id { get; init; }

    [MaxLength(60)]
    public required string Description { get; init; }

    public bool Active { get; init; } = true;

    public string Display
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append($"{Id} – {Description}");
            if (!Active) sn.Append(" [Inactive]");
            return sn.ToString();
        }
    }
}

public interface ISicCodeRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Determines whether the <see cref="SicCode"/> with the given <paramref name="id"/> exists.
    /// </summary>
    /// <param name="id">The ID of the SicCode.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>True if the SicCode exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(string id, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="SicCode"/> with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the SicCode.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{SicCode}">Thrown if no entity exists with the given Id.</exception>
    /// <returns>An SicCode.</returns>
    Task<SicCode> GetAsync(string id, CancellationToken token = default);

    /// <summary>
    /// Returns a read-only collection of all active <see cref="SicCode"/> values.
    /// Returns an empty collection if none exists.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of SicCode entities.</returns>
    Task<IReadOnlyCollection<SicCode>> GetActiveListAsync(CancellationToken token = default);
}
