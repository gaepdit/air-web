using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public class FceManager(IFceRepository repository, IFacilityService facilityService) : IFceManager
{
    public async Task LoadFacilityAsync(Fce fce, CancellationToken token = default) =>
        fce.Facility = await facilityService.GetAsync((FacilityId)fce.FacilityId, token)
            .ConfigureAwait(false);

    public async Task<Fce> CreateAsync(FacilityId facilityId, int year, ApplicationUser? user,
        CancellationToken token = default)
    {
        if (!await facilityService.ExistsAsync(facilityId, token).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        var fce = new Fce(repository.GetNextId(), facilityId, year);
        fce.SetCreator(user?.Id);

        return fce;
    }

    public void Delete(Fce fce, string? comment, ApplicationUser? user)
    {
        fce.SetDeleted(user?.Id);
        fce.DeletedBy = user;
        fce.DeleteComments = comment;
    }

    public void Restore(Fce fce)
    {
        fce.SetNotDeleted();
        fce.DeleteComments = null;
    }
}
