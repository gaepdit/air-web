using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public interface ICaseFileManager
{
    /// <summary>
    /// Creates a new <see cref="CaseFile"/>.
    /// </summary>
    /// <param name="facilityId">The <see cref="FacilityId"/> of the <see cref="Facility"/> to create the Case File for.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The created Case File.</returns>
    Task<CaseFile> CreateCaseFileAsync(FacilityId facilityId, ApplicationUser? user, CancellationToken token = default);

    /// <summary>
    /// Updates the properties of a <see cref="CaseFile"/> to indicate that it was completed and closed.
    /// </summary>
    /// <param name="caseFile">The Case File that was closed.</param>
    /// <param name="user">The user committing the change.</param>
    void CloseCaseFile(CaseFile caseFile, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="CaseFile"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="caseFile">The Case File that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void ReopenCaseFile(CaseFile caseFile, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="CaseFile"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="caseFile">The Case File to delete.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void DeleteCaseFile(CaseFile caseFile, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="CaseFile"/> to indicate that it was restored.
    /// </summary>
    /// <param name="caseFile">The Case File to restore.</param>
    void RestoreCaseFile(CaseFile caseFile);
}
