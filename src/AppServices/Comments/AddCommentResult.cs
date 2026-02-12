using AirWeb.Core.Entities;

namespace AirWeb.AppServices.Comments;

public record AddCommentResult(Guid CommentId, ApplicationUser? CommentUser);
