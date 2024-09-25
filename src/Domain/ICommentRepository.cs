using AirWeb.Domain.Comments;

namespace AirWeb.Domain;

public interface ICommentRepository<in TKey>
{
    /// <summary>
    /// Adds a <see cref="Comment"/> to an <see cref="Entity{TKey}"/> that has comments.
    /// </summary>
    /// <param name="itemId">The ID of the Entity.</param>
    /// <param name="comment">The Comment to add.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task AddCommentAsync(TKey itemId, Comment comment, CancellationToken token = default);

    /// <summary>
    /// Deletes a <see cref="Comment"/> from an <see cref="Entity{TKey}"/>.
    /// </summary>
    /// <param name="commentId">The ID of the Comment to delete.</param>
    /// <param name="userId">The ID of the user deleting the Comment.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default);
}
