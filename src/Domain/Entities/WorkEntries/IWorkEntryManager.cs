﻿using AirWeb.Domain.Entities.EntryActions;
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
    BaseWorkEntry Create(WorkEntryType type,ApplicationUser? user);

    /// <summary>
    /// Creates a new <see cref="EntryAction"/>.
    /// </summary>
    /// <param name="baseWorkEntry">The <see cref="BaseWorkEntry"/> this Action belongs to.</param>
    /// <param name="user">The user creating the entity.</param>
    /// <returns>The WorkEntryAction that was created.</returns>
    EntryAction CreateEntryAction(BaseWorkEntry baseWorkEntry,  ApplicationUser? user);
    
    /// <summary>
    /// Updates the properties of a <see cref="BaseWorkEntry"/> to indicate that it was reviewed and closed.
    /// </summary>
    /// <param name="baseWorkEntry">The Entry that was closed.</param>
    /// <param name="comment">A comment entered by the user committing the change.</param>
    /// <param name="user">The user committing the change.</param>
    void Close(BaseWorkEntry baseWorkEntry, string? comment, ApplicationUser? user);

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
    /// <param name="user">The user committing the change.</param>
    void Restore(BaseWorkEntry baseWorkEntry, ApplicationUser? user);
}
