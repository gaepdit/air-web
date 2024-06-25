using AirWeb.Domain.ExternalEntities.Facilities;

namespace AirWeb.Domain.Entities.Fces;

public interface IFceRepository : IRepository<Fce, int>
{
    /// <summary>
    /// Returns a boolean indicating whether an <see cref="Fce"/> with the given parameters exists.
    /// </summary>
    /// <param name="facilityId">The ID of the facility.</param>
    /// <param name="year">The FCE year.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>True if the FCE exists; otherwise false.</returns>
    public Task<bool> ExistsAsync(FacilityId facilityId, int year, CancellationToken token = default);
}
