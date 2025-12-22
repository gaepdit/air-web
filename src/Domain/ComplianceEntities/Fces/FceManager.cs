using AirWeb.Domain.AuditPoints;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public sealed class FceManager(IFceRepository repository, IFacilityService facilityService) : IFceManager
{
    public async Task<Fce> CreateAsync(FacilityId facilityId, int year, ApplicationUser? user)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        var actionNumber = await facilityService.GetNextActionNumberAsync(facilityId).ConfigureAwait(false);

        var fce = new Fce(repository.GetNextId(), facilityId, year, user) { ActionNumber = actionNumber };
        fce.AuditPoints.Add(FceAuditPoint.Added(user));
        return fce;
    }

    public void Update(Fce fce, ApplicationUser? user)
    {
        fce.SetUpdater(user?.Id);
        fce.AuditPoints.Add(FceAuditPoint.Edited(user));
    }

    public void Delete(Fce fce, string? comment, ApplicationUser? user)
    {
        fce.Delete(comment, user);
        fce.AuditPoints.Add(FceAuditPoint.Deleted(user));
    }

    public void Restore(Fce fce, ApplicationUser? user)
    {
        fce.Undelete();
        fce.AuditPoints.Add(FceAuditPoint.Restored(user));
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
