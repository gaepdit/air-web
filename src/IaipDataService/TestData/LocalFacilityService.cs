using IaipDataService.Facilities;
using System.Collections.ObjectModel;

namespace IaipDataService.TestData;

public sealed class LocalFacilityService : IFacilityService
{
    private IReadOnlyCollection<Facility> Items { get; } = FacilityData.GetData.ToList();

    public Task<Facility> GetAsync(FacilityId id) =>
        Task.FromResult(Items.Single(facility => facility.Id.Equals(id)));

    public Task<Facility?> FindAsync(FacilityId? id) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public async Task<string> GetNameAsync(string id) => (await GetAsync((FacilityId)id)).Name;

    public Task<bool> ExistsAsync(FacilityId id) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync() =>
        Task.FromResult(new ReadOnlyDictionary<FacilityId, string>(
            Items.OrderBy(facility => facility.Id)
                .ToDictionary(facility => facility.Id, facility => facility.Name)));
}
