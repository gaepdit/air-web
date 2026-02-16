using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.Comments;

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

public interface ICaseFileCommentRepository : ICommentRepository<CaseFileComment>;
