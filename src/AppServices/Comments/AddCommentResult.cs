using AirWeb.AppServices.AppNotifications;

namespace AirWeb.AppServices.Comments;

public record AddCommentResult(Guid CommentId, AppNotificationResult AppNotificationResult);
