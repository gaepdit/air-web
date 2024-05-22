using AirWeb.Domain.Entities.Facilities;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.Fces;

public class FceManager(IWorkEntryRepository repository) : IFceManager
{
    public Fce Create(FacilityId facility, int year, ApplicationUser? user)
    {
        var id = repository.GetNextId();
        var item = new Fce(id);
        item.SetCreator(user?.Id);
        return item;
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
