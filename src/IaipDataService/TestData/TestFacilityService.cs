using IaipDataService.Facilities;
using System.Collections.ObjectModel;

namespace IaipDataService.TestData;

public sealed class TestFacilityService : IFacilityService
{
    internal IReadOnlyCollection<Facility> Items { get; } = FacilityData.GetData.ToList();

    private ReadOnlyDictionary<FacilityId, string> FacilityList =>
        new(Items.OrderBy(facility => facility.FacilityId)
            .ToDictionary(facility => facility.Id, facility => facility.Name));

    public Task<Facility?> FindFacilityDetailsAsync(FacilityId id, bool forceRefresh = false) =>
        FindFacility(id);

    public Task<Facility?> FindFacilitySummaryAsync(FacilityId id, bool forceRefresh = false) =>
        FindFacility(id);

    private Task<Facility?> FindFacility(FacilityId id) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public Task<string> GetNameAsync(string id) =>
        FacilityList.TryGetValue((FacilityId)id, out var name)
            ? Task.FromResult(name)
            : throw new InvalidOperationException("Facility not found.");

    public Task<bool> ExistsAsync(FacilityId id) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public async Task<ushort> GetNextActionNumberAsync(FacilityId id)
    {
        var facility = await FindFacility(id);
        return facility is null
            ? throw new ArgumentException($"Facility not found: {id}")
            : facility.NextActionNumber++;
    }

    public Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync(bool forceRefresh = false) =>
        Task.FromResult(FacilityList);
}
