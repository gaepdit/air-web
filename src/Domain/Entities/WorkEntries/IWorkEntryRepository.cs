using MyApp.Domain.Entities.EntryActions;

namespace MyApp.Domain.Entities.WorkEntries;

public interface IWorkEntryRepository : IRepository<WorkEntry>
{
    /// <summary>
    /// Returns the <see cref="WorkEntry"/> with the given <paramref name="id"/> and includes all additional
    /// properties (<see cref="EntryAction"/>). Returns null if there are no matches.
    /// </summary>
    /// <param name="id">The Id of the WorkEntry.</param>
    /// <param name="includeDeletedActions">Whether to include deleted WorkEntry Actions in the result.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>A WorkEntry entity.</returns>
    Task<WorkEntry?> FindIncludeAllAsync(Guid id, bool includeDeletedActions = false, CancellationToken token = default);
}
