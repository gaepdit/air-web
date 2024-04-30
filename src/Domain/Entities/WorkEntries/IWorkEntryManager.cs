using MyApp.Domain.Entities.EntryActions;
using MyApp.Domain.Identity;

namespace MyApp.Domain.Entities.WorkEntries;

public interface IWorkEntryManager
{
    /// <summary>
    /// Creates a new <see cref="WorkEntry"/>.
    /// </summary>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The Entry that was created.</returns>
    WorkEntry Create(ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="EntryAction"/>.
    /// </summary>
    /// <param name="workEntry">The <see cref="WorkEntry"/> this Action belongs to.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The WorkEntryAction that was created.</returns>
    EntryAction CreateEntryAction(WorkEntry workEntry,  ApplicationUser? user);
    
    /// <summary>
    /// Updates the properties of a <see cref="WorkEntry"/> to indicate that it was reviewed and closed.
    /// </summary>
    /// <param name="workEntry">The Entry that was closed.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(WorkEntry workEntry, string? comment, ApplicationUser? user);

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
    /// <param name="user">The user committing the change.</param>
    void Restore(WorkEntry workEntry, ApplicationUser? user);
}
