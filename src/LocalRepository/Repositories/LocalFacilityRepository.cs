using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.TestData.Entities;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalFacilityRepository : IFacilityRepository
{
    private IReadOnlyCollection<Facility> Items { get; } = FacilityData.GetData.ToList();

    public Task<bool> FacilityExistsAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public Task<Facility?> FindFacilityAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public Task<Facility> GetFacilityAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(facility => facility.Id.Equals(id)));

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => default;
}
