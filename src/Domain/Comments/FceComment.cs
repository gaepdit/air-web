using AirWeb.Core.Entities;

namespace AirWeb.Domain.Comments;

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

public interface IFceCommentRepository : ICommentRepository<FceComment>;
