using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.Comments;

public record AddCommentResult(Guid CommentId, ApplicationUser? CommentUser);
