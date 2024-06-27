using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.Fces;

public class FceManager(IFceRepository repository) : IFceManager
{
    public Fce Create(Facility facility, int year, ApplicationUser? user)
    {
        var item = new Fce(repository.GetNextId()) { Facility = facility, Year = year };
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
