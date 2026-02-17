using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.Customers;

namespace AirWeb.Domain.Sbeap.Entities.Cases;

public class CaseworkManager : ICaseworkManager
{
    public Casework Create(Customer customer, DateOnly caseOpenedDate, string? createdById)
    {
        var item = new Casework(Guid.NewGuid(), customer, caseOpenedDate);
        item.SetCreator(createdById);
        return item;
    }

    public ActionItem CreateActionItem(Casework casework, ActionItemType actionItemType, string? createdById)
    {
        var item = new ActionItem(Guid.NewGuid(), casework, actionItemType);
        item.SetCreator(createdById);
        return item;
    }
}
