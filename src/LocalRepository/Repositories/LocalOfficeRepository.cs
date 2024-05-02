using AirWeb.Domain.Entities.Offices;
using AirWeb.Domain.Identity;
using AirWeb.LocalRepository.Identity;
using AirWeb.TestData;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalOfficeRepository()
    : NamedEntityRepository<Office>(OfficeData.GetData), IOfficeRepository
{
    public LocalUserStore Staff { get; } = new();

    public Task<List<ApplicationUser>> GetStaffMembersListAsync(Guid id, bool includeInactive,
        CancellationToken token = default) =>
        Task.FromResult(Staff.Users
            .Where(user => user.Office != null && user.Office.Id == id)
            .Where(user => includeInactive || user.Active)
            .OrderBy(user => user.FamilyName).ThenBy(user => user.GivenName).ThenBy(user => user.Id)
            .ToList());
}
