using AirWeb.Domain.Core;

namespace AirWeb.Domain.Sbeap.Entities.Agencies;

public class Agency : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public Agency() { }
    public Agency(Guid id, string name) : base(id, name) { }
}

public interface IAgencyRepository : INamedEntityRepository<Agency>;

public interface IAgencyManager : INamedEntityManager<Agency>;

public class AgencyManager(IAgencyRepository repository)
    : NamedEntityManager<Agency, IAgencyRepository>(repository), IAgencyManager;
