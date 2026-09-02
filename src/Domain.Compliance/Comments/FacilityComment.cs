using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.Comments;

public record FacilityComment : Comment, ISetCommentItemId<FacilityId>
{
    public FacilityComment() { }

    // This constructor is only used for creating test data.
    public FacilityComment(Comment comment, FacilityId facilityId)
    {
        Id = Guid.NewGuid();
        Text = comment.Text;
        CommentBy = comment.CommentBy;
        CommentedAt = comment.CommentedAt;
        FacilityId = facilityId;
    }

    public FacilityId FacilityId { get; private set; }
    public void SetItemId(FacilityId facilityId) => FacilityId = facilityId;
}

public interface IFacilityCommentRepository : ICommentRepository<CaseFileComment>;
