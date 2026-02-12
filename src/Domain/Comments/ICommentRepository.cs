namespace AirWeb.Domain.Comments;

public interface ICommentRepository<in TKey, in TComment> : IDisposable, IAsyncDisposable
    where TKey : IEquatable<TKey>
    where TComment : Comment
{
    /// <summary>
    /// Adds a <see cref="Comment"/> to an <see cref="Entity{TKey}"/> that has comments.
    /// </summary>
    /// <param name="itemId">The ID of the Entity.</param>
    /// <param name="comment">The Comment to add.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task AddCommentAsync(TKey itemId, TComment comment, CancellationToken token = default);

    /// <summary>
    /// Deletes a <see cref="Comment"/> from an <see cref="Entity{TKey}"/>.
    /// </summary>
    /// <param name="commentId">The ID of the Comment to delete.</param>
    /// <param name="userId">The ID of the user deleting the Comment.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default);
}

public interface ICommentRepository<in TComment> : ICommentRepository<int, TComment>
    where TComment : Comment;

public interface ICaseFileCommentRepository : ICommentRepository<CaseFileComment>;

public interface IComplianceWorkCommentRepository : ICommentRepository<ComplianceWorkComment>;

public interface IFceCommentRepository : ICommentRepository<FceComment>;
