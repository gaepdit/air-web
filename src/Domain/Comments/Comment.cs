using AirWeb.Domain.Identity;

namespace AirWeb.Domain.Comments;

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

public record CaseFileComment : Comment, ISetCommentItemId
{
    public CaseFileComment() { }

    // This constructor is only used for creating test data.
    public CaseFileComment(Comment comment, int itemId)
    {
        Id = Guid.NewGuid();
        Text = comment.Text;
        CommentBy = comment.CommentBy;
        CommentedAt = comment.CommentedAt;
        CaseFileId = itemId;
    }

    public int CaseFileId { get; private set; }
    public void SetItemId(int itemId) => CaseFileId = itemId;
}

public record ComplianceWorkComment : Comment, ISetCommentItemId
{
    public ComplianceWorkComment() { }

    // This constructor is only used for creating test data.
    public ComplianceWorkComment(Comment comment, int itemId)
    {
        Id = Guid.NewGuid();
        Text = comment.Text;
        CommentBy = comment.CommentBy;
        CommentedAt = comment.CommentedAt;
        ComplianceWorkId = itemId;
    }

    public int ComplianceWorkId { get; private set; }
    public void SetItemId(int itemId) => ComplianceWorkId = itemId;
}

public record FceComment : Comment, ISetCommentItemId

{
    public FceComment() { }

    // This constructor is only used for creating test data.
    public FceComment(Comment comment, int itemId)
    {
        Id = Guid.NewGuid();
        Text = comment.Text;
        CommentBy = comment.CommentBy;
        CommentedAt = comment.CommentedAt;
        FceId = itemId;
    }

    public int FceId { get; private set; }
    public void SetItemId(int itemId) => FceId = itemId;
}
