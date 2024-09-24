using AirWeb.AppServices.Comments;

namespace AirWeb.WebApp.Models;

public class CommentsSectionModel
{
    public required List<CommentViewDto> Comments { get; init; }
    public required CommentAddDto NewComment { get; init; }
    public Guid NewCommentId { get; init; }
    public string? NotificationFailureMessage { get; init; }
    public bool CanAddComment { get; init; }
    public bool CanDeleteComment { get; init; }
}
