using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalFceRepository()
    : BaseRepository<Fce, int>(FceData.GetData), IFceRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(fce => fce.Id).Max() + 1;

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, CancellationToken token = default) =>
        Task.FromResult(Items.Any(fce => fce.FacilityId == facilityId && fce.Year.Equals(year)));

    public async Task AddCommentAsync(int id, Comment comment, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).Comments.Add(comment);
}
