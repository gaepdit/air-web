namespace AirWeb.Core.Entities;

public class Office : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    public Office() { }
    public Office(Guid id, string name) : base(id, name) { }
}

public interface IOfficeManager : INamedEntityManager<Office>;

public class OfficeManager(IOfficeRepository repository)
    : NamedEntityManager<Office, IOfficeRepository>(repository), IOfficeManager;

public interface IOfficeRepository : INamedEntityRepository<Office>
{
    /// <summary>
    /// Returns a list of all active <see cref="ApplicationUser"/> located in the <see cref="Office"/> with the
    /// given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the Office.</param>
    /// <param name="includeInactive">A flag indicating whether to include inactive Staff Members.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{Office}">Thrown if no entity exists with the given Id.</exception>
    /// <returns>A list of Users.</returns>
    Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default);
}
