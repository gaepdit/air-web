using AirWeb.Domain;

namespace AirWeb.AppServices.Comments;

public interface ICommentService<TKey>
    where TKey : IEquatable<TKey>
{
    Task<AddCommentResult> AddCommentAsync(ICommentRepository<TKey> repository, TKey itemId, CommentAddDto resource,
        CancellationToken token = default);

    Task DeleteCommentAsync(ICommentRepository<TKey> repository, Guid commentId, CancellationToken token = default);
}
