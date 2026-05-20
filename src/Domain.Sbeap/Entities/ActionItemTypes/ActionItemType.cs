using AirWeb.Domain.Core;

namespace AirWeb.Domain.Sbeap.Entities.ActionItemTypes;

public class ActionItemType : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public ActionItemType() { }
    public ActionItemType(Guid id, string name) : base(id, name) { }
}

public interface IActionItemTypeRepository : INamedEntityRepository<ActionItemType>;

public interface IActionItemTypeManager : INamedEntityManager<ActionItemType>;

public class ActionItemTypeManager(IActionItemTypeRepository repository)
    : NamedEntityManager<ActionItemType, IActionItemTypeRepository>(repository), IActionItemTypeManager;
