using IaipDataService.Facilities;

namespace IaipDataService.TestData;

public sealed class TestFacilityService : IFacilityService
{
    internal IReadOnlyCollection<Facility> Items { get; } = [.. FacilityData.GetData];

    public Task<Facility?> FindFacilityAsync(FacilityId id, bool forceRefresh = false) =>
        FindFacility(id);

    public Task<Facility?> FindFacilityDetailsAsync(FacilityId id, bool forceRefresh = false) =>
        FindFacility(id);

    private Task<Facility?> FindFacility(FacilityId id) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public async Task<string> GetNameAsync(string id) =>
        (await GetAllAsync().ConfigureAwait(false)).SingleOrDefault(f => f.FacilityId == id)?.Name ??
            throw new InvalidOperationException("Facility not found.");

    public Task<bool> ExistsAsync(FacilityId id) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public async Task<ushort> GetNextActionNumberAsync(FacilityId id)
    {
        var facility = await FindFacility(id).ConfigureAwait(false);
        return facility is null
            ? throw new ArgumentException($"Facility not found: {id}")
            : facility.NextActionNumber++;
    }

    public Task<IReadOnlyCollection<FacilitySummary>> GetAllAsync(bool forceRefresh = false) =>
        Task.FromResult(Items.Select(f => new FacilitySummary(f)).ToList() as IReadOnlyCollection<FacilitySummary>);
}
