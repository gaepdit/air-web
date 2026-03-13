using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.Domain.Sbeap.Entities.Cases;
using AirWeb.TestData.Sbeap;

namespace AirWeb.MemRepository.SbeapRepositories;

public sealed class CaseworkMemRepository(IActionItemRepository actionItemRepository)
    : BaseRepository<Casework, Guid>(CaseworkData.GetCases), ICaseworkRepository
{
    public async Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default)
    {
        var result = await FindAsync(id, token).ConfigureAwait(false);
        if (result is null) return result;

        result.ActionItems = (await actionItemRepository
                .GetListAsync(e => e.Casework.Id == result.Id && !e.IsDeleted, token).ConfigureAwait(false))
            .OrderByDescending(i => i.ActionDate)
            .ThenByDescending(i => i.EnteredOn)
            .ToList();

        return result;
    }
}
