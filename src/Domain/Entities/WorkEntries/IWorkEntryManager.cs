using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.WorkEntries;

public interface IWorkEntryManager
{
    /// <summary>
    /// Creates a new <see cref="BaseWorkEntry"/>.
    /// </summary>
    /// <param name="type">The <see cref="WorkEntryType"/> of the Work Entry to create.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Work Entry that was created.</returns>
    BaseWorkEntry CreateWorkEntry(WorkEntryType type, ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="BaseComplianceEvent"/>.
    /// </summary>
    /// <param name="type">The <see cref="ComplianceEventType"/> of the Compliance Event to create.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Compliance Event that was created.</returns>
    BaseComplianceEvent CreateComplianceEvent(ComplianceEventType type, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="BaseWorkEntry"/> to indicate that it was reviewed and closed.
    /// </summary>
    /// <param name="baseWorkEntry">The Entry that was closed.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(BaseWorkEntry baseWorkEntry, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="BaseWorkEntry"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="baseWorkEntry">The Entry that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void Reopen(BaseWorkEntry baseWorkEntry, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="BaseWorkEntry"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="baseWorkEntry">The Entry which was deleted.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(BaseWorkEntry baseWorkEntry, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="BaseWorkEntry"/> to indicate that it was restored.
    /// </summary>
    /// <param name="baseWorkEntry">The Entry which was restored.</param>
    void Restore(BaseWorkEntry baseWorkEntry);
}
