using AirWeb.Domain.Core.Entities;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.MemRepository.CommonRepositories;

[SuppressMessage("", "S2436")]
public abstract class CommentMemRepository<TEntity, TKey, TComment>(IReadRepository<TEntity, TKey> itemRepository)
    : ICommentRepository<TKey, TComment>
    where TEntity : class, IEntity<TKey>, IComments<TComment>
    where TKey : IEquatable<TKey>
    where TComment : Comment
{
    public async Task AddCommentAsync(TKey itemId, TComment comment, CancellationToken token = default)
    {
        var entity = await itemRepository
                         .FindAsync(entity => entity.Id.Equals(itemId), token).ConfigureAwait(false) ??
                     throw new EntityNotFoundException<TEntity>(itemId);
        entity.Comments.Add(comment);
    }

    public async Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var entity = await itemRepository
            .FindAsync(entity => entity.Comments.Any(comment => comment.Id == commentId), token).ConfigureAwait(false);

        var comment = entity?.Comments.FirstOrDefault(comment => comment.Id == commentId);
        if (comment == null) return;

        entity!.Comments.Remove(comment);
    }

    #region IDisposable,  IAsyncDisposable

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // ReSharper disable once UnusedParameter.Global
    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    #endregion
}

public abstract class CommentMemRepository<TEntity, TComment>(IReadRepository<TEntity, int> itemRepository)
    : CommentMemRepository<TEntity, int, TComment>(itemRepository), ICommentRepository<TComment>
    where TEntity : class, IEntity<int>, IComments<TComment>
    where TComment : Comment;
