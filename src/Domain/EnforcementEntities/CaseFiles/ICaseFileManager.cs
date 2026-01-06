using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

public interface ICaseFileManager : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Creates a new <see cref="CaseFile"/>.
    /// </summary>
    /// <param name="facilityId">The <see cref="FacilityId"/> of the <see cref="Facility"/> to create the Case File for.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The created Case File.</returns>
    Task<CaseFile> CreateAsync(FacilityId facilityId, ApplicationUser? user, CancellationToken token = default);

    /// <summary>
    /// Updates the properties of a <see cref="CaseFile"/> to indicate that it was completed and closed.
    /// </summary>
    /// <param name="caseFile">The Case File that was closed.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(CaseFile caseFile, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="CaseFile"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="caseFile">The Case File that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void Reopen(CaseFile caseFile, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="CaseFile"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="caseFile">The Case File to delete.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(CaseFile caseFile, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="CaseFile"/> to indicate that it was restored.
    /// </summary>
    /// <param name="caseFile">The Case File to restore.</param>
    /// <param name="user"></param>
    void Restore(CaseFile caseFile, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="CaseFile"/> to indicate that it was edited.
    /// </summary>
    /// <param name="caseFile">The Case File that was edited.</param>
    /// <param name="user">The user committing the change.</param>
    void Update(CaseFile caseFile, ApplicationUser? user);

    /// <summary>
    /// Links a <see cref="CaseFile"/> to a <see cref="ComplianceEvent"/>.
    /// </summary>
    /// <param name="caseFile">The Case File to update.</param>
    /// <param name="entry">The Compliance Event to link.</param>
    /// <param name="user">The user committing the change.</param>
    void LinkComplianceEvent(CaseFile caseFile, ComplianceEvent entry, ApplicationUser? user);

    /// <summary>
    /// Unlinks a <see cref="CaseFile"/> from a <see cref="ComplianceEvent"/>.
    /// </summary>
    /// <param name="caseFile">The Case File to update.</param>
    /// <param name="entry">The Compliance Event to unlink.</param>
    /// <param name="user">The user committing the change.</param>
    void UnlinkComplianceEvent(CaseFile caseFile, ComplianceEvent entry, ApplicationUser? user);

    /// <summary>
    /// Update the <see cref="Pollutant"/> and <see cref="AirProgram"/> lists for a <see cref="CaseFile"/>.
    /// </summary>
    /// <param name="caseFile">The Case File to update.</param>
    /// <param name="pollutants">The Pollutant IDs as strings.</param>
    /// <param name="airPrograms">The Air Programs.</param>
    /// <param name="user">The user committing the change.</param>
    void UpdatePollutantsAndPrograms(CaseFile caseFile, IEnumerable<string> pollutants,
        IEnumerable<AirProgram> airPrograms, ApplicationUser? user);
}
