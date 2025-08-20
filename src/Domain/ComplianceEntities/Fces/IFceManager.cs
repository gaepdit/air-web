using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.Fces;

public interface IFceManager : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Creates a new <see cref="Fce"/>.
    /// </summary>
    /// <param name="facilityId">The <see cref="FacilityId"/> of the <see cref="Facility"/> to create the FCE for.</param>
    /// <param name="year">The year to create the FCE for.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The created FCE.</returns>
    Task<Fce> CreateAsync(FacilityId facilityId, int year, ApplicationUser? user, CancellationToken token = default);

    /// <summary>
    /// Updates the properties of a <see cref="Fce"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="fce">The FCE to delete.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(Fce fce, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="Fce"/> to indicate that it was restored.
    /// </summary>
    /// <param name="fce">The FCE to restore.</param>
    void Restore(Fce fce);
}
