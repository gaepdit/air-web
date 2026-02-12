using AirWeb.Core.Entities;

namespace AirWeb.Domain.Comments;

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

public interface IComplianceWorkCommentRepository : ICommentRepository<ComplianceWorkComment>;
