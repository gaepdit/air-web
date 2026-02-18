using AirWeb.Domain.Core.Entities;
using AirWeb.MemRepository.Identity;
using AirWeb.TestData.Identity;

namespace AirWeb.MemRepository.Repositories;

public sealed class OfficeMemRepository()
    : NamedEntityRepository<Office>(OfficeData.GetData), IOfficeRepository
{
    public LocalUserStore Staff { get; } = new();

    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Task.FromResult(Staff.Users
            .Where(user => user.Office != null && user.Office.Id.Equals(id))
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToList());
}
