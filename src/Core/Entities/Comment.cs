namespace AirWeb.Core.Entities;

public record Comment : ISoftDelete<string>
{
    protected Comment() { }

    // This constructor is only used for creating test data and for unit testing.
    public Comment(string text, ApplicationUser? user)
    {
        Id = Guid.NewGuid();
        Text = text;
        CommentBy = user;
    }

    // Properties
    public Guid Id { get; init; }

    [StringLength(15_000)]
    public string Text { get; init; } = null!;

    public ApplicationUser? CommentBy { get; init; }
    public DateTimeOffset CommentedAt { get; init; } = DateTimeOffset.Now;

    // Soft delete properties
    public bool IsDeleted => DeletedAt.HasValue;
    public DateTimeOffset? DeletedAt { get; private set; }
    public string? DeletedById { get; private set; }

    public void SetDeleted(string? userId)
    {
        DeletedAt = DateTimeOffset.Now;
        DeletedById = userId;
    }

    public void SetNotDeleted()
    {
        DeletedAt = null;
        DeletedById = null;
    }
}

public interface ISetCommentItemId<in TKey>
    where TKey : IEquatable<TKey>
{
    void SetItemId(TKey itemId);
}

public interface ISetCommentItemId : ISetCommentItemId<int>;

public interface IComments<TComment> where TComment : Comment
{
    List<TComment> Comments { get; }
}

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
