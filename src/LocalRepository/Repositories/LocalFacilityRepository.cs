using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.TestData.ExternalEntities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalFacilityRepository : IFacilityRepository
{
    private IReadOnlyCollection<Facility> Items { get; } = FacilityData.GetData.ToList();

    public Task<bool> FacilityExistsAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public Task<Facility?> FindFacilityAsync(FacilityId? id, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public Task<Facility> GetFacilityAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(facility => facility.Id.Equals(id)));

    public Task<string> GetFacilityNameAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(facility => facility.Id.Equals(id)).CompanyName);

    public Task<Dictionary<string, string>> GetFacilityNamesAsync(string[] facilityIds, CancellationToken token) =>
        Task.FromResult(Items.Where(facility => facilityIds.Contains(facility.Id.ToString()))
            .ToDictionary(facility => facility.Id.ToString(), facility => facility.CompanyName));

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;
}
