using AirWeb.Domain.Core.Entities;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.Repositories;

public sealed class OfficeRepository(AppDbContext context)
    : NamedEntityRepository<Office, AppDbContext>(context), IOfficeRepository
{
    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Context.Set<ApplicationUser>().AsNoTracking()
            .Where(user => user.Office != null && user.Office.Id.Equals(id))
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToListAsync(token);
}
