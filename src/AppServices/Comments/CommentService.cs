using AirWeb.AppServices.IdentityServices;
using AirWeb.Domain;
using AirWeb.Domain.Comments;

namespace AirWeb.AppServices.Comments;

public class CommentService<TKey>(IUserService userService) : ICommentService<TKey>
    where TKey : IEquatable<TKey>
{
    public async Task<AddCommentResult> AddCommentAsync(ICommentRepository<TKey> repository, TKey itemId,
        CommentAddDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var comment = Comment.CreateComment(resource.Comment, currentUser);
        await repository.AddCommentAsync(itemId, comment, token).ConfigureAwait(false);
        return new AddCommentResult(comment.Id, currentUser);
    }

    public async Task DeleteCommentAsync(ICommentRepository<TKey> repository, Guid commentId,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        await repository.DeleteCommentAsync(commentId, currentUser?.Id, token).ConfigureAwait(false);
    }
}
