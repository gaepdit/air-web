using IaipDataService.Facilities;

namespace IaipDataService.TestData;

public sealed class TestFacilityService : IFacilityService
{
    internal IReadOnlyCollection<Facility> Items { get; } = [.. FacilityData.GetData];

    public Task<Facility?> FindFacilityAsync(FacilityId id, bool forceRefresh = false,
        CancellationToken token = default) =>
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

    public Task<DateTime?> GetFacilityEpaDxDateAsync(FacilityId id,
        CancellationToken token = default)
    {
        var start = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Unspecified);
        var totalDays = Convert.ToInt32((DateTime.Today - start).TotalDays);
        var dxDate = start.AddDays(Random.Shared.Next(totalDays)).AddMinutes(Random.Shared.Next(1440));
        return Task.FromResult<DateTime?>(dxDate);
    }

    public Task<bool> RefreshEpaDataExchange(FacilityId id) => Task.FromResult(true);

    public Task<IReadOnlyCollection<FacilitySummary>> GetAllAsync(bool forceRefresh = false,
        bool includePortableSources = true, CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<FacilitySummary>>(Items
            .Where(f => includePortableSources || f.Id.CountyCode != "777")
            .Select(f => new FacilitySummary(f))
            .ToList());
}
