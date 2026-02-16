using AirWeb.Domain.Core.Entities;

namespace AirWeb.AppServices.Core.EntityServices.Comments;

public record AddCommentResult(Guid CommentId, ApplicationUser? CommentUser);
