namespace AirWeb.Domain.Lookups.Offices;

public class Office : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public Office() { }
    internal Office(Guid id, string name) : base(id, name) { }
}

public interface IOfficeManager : INamedEntityManager<Office>;

public class OfficeManager(IOfficeRepository repository)
    : NamedEntityManager<Office, IOfficeRepository>(repository), IOfficeManager;
