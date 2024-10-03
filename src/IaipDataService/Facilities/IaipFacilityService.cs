namespace IaipDataService.Facilities;

public sealed class IaipFacilityService : IFacilityService
{
    public Task<Facility> GetAsync(FacilityId id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Facility?> FindAsync(FacilityId? id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(FacilityId id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Facility>> GetListAsync(CancellationToken token = default)
    {
        // This method is only available when using in-memory data, and won't be implemented
        // for accessing from the airbranch DB. A feature enhancement has been suggested for
        // replacing this with a search tool.
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}
