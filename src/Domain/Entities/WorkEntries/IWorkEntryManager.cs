using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Identity;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.Domain.Entities.WorkEntries;

public interface IWorkEntryManager
{
    /// <summary>
    /// Creates a new <see cref="BaseWorkEntry"/>.
    /// </summary>
    /// <param name="type">The <see cref="WorkEntryType"/> of the Work Entry to create.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <param name="complianceEventType">The <see cref="ComplianceEventType"/> of the entry if it is a Compliance Event.</param>
    /// <param name="notificationType">The <see cref="NotificationType"/> of the entry if it is a Notification.</param>
    /// <returns>The Work Entry that was created.</returns>
    BaseWorkEntry Create(WorkEntryType type, ApplicationUser? user, ComplianceEventType? complianceEventType = null,
        NotificationType? notificationType = null);

    /// <summary>
    /// Creates a new <see cref="Comment"/>.
    /// </summary>
    /// <param name="text">The text of the comment.</param>
    /// <param name="user">The <see cref="ApplicationUser"/> who wrote the comment.</param>
    /// <returns>The Comment that was created.</returns>
    Comment CreateComment(string text, ApplicationUser? user);

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
