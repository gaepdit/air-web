using AirWeb.Domain.Identity;

namespace AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;

public interface IComplianceWorkManager : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Creates a new <see cref="ComplianceWork"/>.
    /// </summary>
    /// <param name="type">The <see cref="ComplianceWorkType"/> of the Work Entry to create.</param>
    /// <param name="facilityId">The ID of the facility associated with the Work Entry.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Work Entry that was created.</returns>
    Task<ComplianceWork> CreateAsync(ComplianceWorkType type, FacilityId facilityId, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="ComplianceWork"/> to indicate that it was edited.
    /// </summary>
    /// <param name="complianceWork">The Entry that was updated.</param>
    /// <param name="user">The user committing the change.</param>
    void Update(ComplianceWork complianceWork, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="ComplianceWork"/> to indicate that it was reviewed and closed.
    /// </summary>
    /// <param name="complianceWork">The Entry that was closed.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(ComplianceWork complianceWork, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="ComplianceWork"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="complianceWork">The Entry that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void Reopen(ComplianceWork complianceWork, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="ComplianceWork"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="complianceWork">The Entry which was deleted.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(ComplianceWork complianceWork, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="ComplianceWork"/> to indicate that it was restored.
    /// </summary>
    /// <param name="complianceWork">The Entry which was restored.</param>
    /// <param name="user"></param>
    void Restore(ComplianceWork complianceWork, ApplicationUser? user);
}
