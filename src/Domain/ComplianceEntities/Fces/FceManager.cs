using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public sealed class FceManager(IFceRepository repository, IFacilityService facilityService) : IFceManager
{
    public async Task<Fce> CreateAsync(FacilityId facilityId, int year, ApplicationUser? user,
        CancellationToken token = default)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        return new Fce(repository.GetNextId(), facilityId, year, user);
    }

    public void Delete(Fce fce, string? comment, ApplicationUser? user) => fce.Delete(comment, user);

    public void Restore(Fce fce) => fce.Undelete();

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
