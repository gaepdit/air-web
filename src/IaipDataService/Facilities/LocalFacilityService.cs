﻿namespace IaipDataService.Facilities;

public sealed class LocalFacilityService : IFacilityService
{
    private IReadOnlyCollection<Facility> Items { get; } = FacilityData.GetData.ToList();

    public Task<Facility> GetAsync(FacilityId id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(facility => facility.Id.Equals(id)));

    public Task<Facility?> FindAsync(FacilityId? id) =>
        Task.FromResult(Items.SingleOrDefault(facility => facility.Id.Equals(id)));

    public Task<bool> ExistsAsync(FacilityId id) =>
        Task.FromResult(Items.Any(facility => facility.Id == id));

    public Task<IReadOnlyCollection<Facility>> GetListAsync(CancellationToken token = default) =>
        Task.FromResult(Items);
}
