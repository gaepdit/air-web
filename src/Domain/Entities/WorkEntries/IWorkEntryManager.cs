using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Entities.WorkEntries;

public interface IWorkEntryManager
{
    /// <summary>
    /// Creates a new <see cref="WorkEntry"/>.
    /// </summary>
    /// <param name="type">The <see cref="WorkEntryType"/> of the Work Entry to create.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <param name="complianceEventType">The <see cref="ComplianceEventType"/> of the entry if it is a Compliance Event.</param>
    /// <param name="notificationType">The <see cref="NotificationType"/> of the entry if it is a Notification.</param>
    /// <returns>The Work Entry that was created.</returns>
    WorkEntry Create(WorkEntryType type, ApplicationUser? user, ComplianceEventType? complianceEventType = null,
        NotificationType? notificationType = null);

    /// <summary>
    /// Updates the properties of a <see cref="WorkEntry"/> to indicate that it was reviewed and closed.
    /// </summary>
    /// <param name="workEntry">The Entry that was closed.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(WorkEntry workEntry, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a closed <see cref="WorkEntry"/> to indicate that it was reopened.
    /// </summary>
    /// <param name="workEntry">The Entry that was reopened.</param>
    /// <param name="user">The user committing the change.</param>
    void Reopen(WorkEntry workEntry, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a <see cref="WorkEntry"/> to indicate that it was deleted.
    /// </summary>
    /// <param name="workEntry">The Entry which was deleted.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Delete(WorkEntry workEntry, string? comment, ApplicationUser? user);

    /// <summary>
    /// Updates the properties of a deleted <see cref="WorkEntry"/> to indicate that it was restored.
    /// </summary>
    /// <param name="workEntry">The Entry which was restored.</param>
    void Restore(WorkEntry workEntry);
}
