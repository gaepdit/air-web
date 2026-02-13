using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Core.Entities;

namespace AirWeb.AppServices.Core.EntityServices.Comments;

public interface ICommentService<in TKey> where TKey : IEquatable<TKey>
{
    Task<AddCommentResult> AddCommentAsync(TKey itemId, CommentAddDto resource, CancellationToken token = default);
    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}

public interface ICommentService : ICommentService<int>;

public abstract class CommentService<TKey, TComment>(
    ICommentRepository<TKey, TComment> repository,
    IUserService userService) : ICommentService<TKey>
    where TKey : IEquatable<TKey>
    where TComment : Comment, ISetCommentItemId<TKey>, new()
{
    public async Task<AddCommentResult> AddCommentAsync(TKey itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var comment = new TComment
        {
            Id = Guid.NewGuid(),
            Text = resource.Comment,
            CommentBy = currentUser,
        };
        comment.SetItemId(itemId);
        await repository.AddCommentAsync(itemId, comment, token).ConfigureAwait(false);
        return new AddCommentResult(comment.Id, currentUser);
    }

    public async Task DeleteCommentAsync(Guid commentId, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        await repository.DeleteCommentAsync(commentId, currentUser?.Id, token).ConfigureAwait(false);
    }
}

public abstract class CommentService<TComment>(ICommentRepository<TComment> repository, IUserService userService)
    : CommentService<int, TComment>(repository, userService), ICommentService
    where TComment : Comment, ISetCommentItemId, new();

public interface ICaseFileCommentService : ICommentService;
