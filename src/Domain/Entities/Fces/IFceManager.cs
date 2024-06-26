using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.Fces;

public interface IFceManager
{
    /// <summary>
    /// Creates a new <see cref="Fce"/>.
    /// </summary>
    /// <param name="facility">The <see cref="FacilityId"/> to create the FCE for.</param>
    /// <param name="year">The year to create the FCE for.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The created FCE.</returns>
    Fce Create(FacilityId facility, int year, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="Fce"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="fce">The Entry which was deleted.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(Fce fce, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="Fce"/> to indicate that it was restored.
    /// </summary>
    /// <param name="fce">The Entry which was restored.</param>
    void Restore(Fce fce);
}
