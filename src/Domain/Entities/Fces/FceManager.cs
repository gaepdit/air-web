using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.Fces;

public class FceManager(IFceRepository repository, IFacilityRepository facilityRepository) : IFceManager
{
    public async Task LoadFacilityAsync(Fce fce, CancellationToken token = default) =>
        fce.Facility = await facilityRepository.GetFacilityAsync((FacilityId)fce.FacilityId, token)
            .ConfigureAwait(false);

    public async Task<Fce> CreateAsync(FacilityId facilityId, int year, ApplicationUser? user,
        CancellationToken token = default)
    {
        if (!await facilityRepository.FacilityExistsAsync(facilityId, token).ConfigureAwait(false))
            throw new ArgumentException("Facility does not exist.", nameof(facilityId));

        var item = new Fce(repository.GetNextId(), facilityId, year);
        item.SetCreator(user?.Id);
        return item;
    }

    public Comment CreateComment(string text, ApplicationUser? user) =>
        new()
        {
            Id = Guid.NewGuid(),
            Text = text,
            CommentBy = user,
        };

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
