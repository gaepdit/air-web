using IaipDataService.Facilities;
using System.Collections.ObjectModel;

namespace IaipDataService.TestData;

public sealed class LocalFacilityService : IFacilityService
{
    private IReadOnlyCollection<Facility> Items { get; } = FacilityData.GetData.ToList();
    private ReadOnlyDictionary<FacilityId, string> FacilityList { get; } = FacilityData.GetFacilityList();

    public Task<Facility> GetAsync(FacilityId id, bool forceRefresh = false) =>
        Task.FromResult(Items.Single(facility => facility.Id.Equals(id)));

    public Task<Facility?> FindAsync(FacilityId? id, bool forceRefresh = false) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public Task<string> GetNameAsync(string id) =>
        FacilityList.TryGetValue((FacilityId)id, out var name)
            ? Task.FromResult(name)
            : throw new InvalidOperationException("Facility not found.");

    public Task<bool> ExistsAsync(FacilityId id) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync(bool forceRefresh = false) =>
        Task.FromResult(FacilityList);
}
