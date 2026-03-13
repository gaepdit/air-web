using AirWeb.Domain.Core.Entities;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.EfRepository.CommonRepositories;

[SuppressMessage("", "S2436")]
public abstract class CommentRepository<TEntity, TKey, TComment, TContext>(TContext context)
    : ICommentRepository<TKey, TComment>
    where TEntity : class, IEntity<TKey>, IComments<TComment>
    where TKey : IEquatable<TKey>
    where TComment : Comment
    where TContext : DbContext
{
    private TContext Context => context;

    public async Task AddCommentAsync(TKey itemId, TComment comment, CancellationToken token = default)
    {
        var entity = await Context.Set<TEntity>().FindAsync(keyValues: [itemId], token).ConfigureAwait(false) ??
                     throw new EntityNotFoundException<TEntity>(itemId);
        entity.Comments.Add(comment);
        await Context.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = await Context.Set<TComment>().FindAsync(keyValues: [commentId], token).ConfigureAwait(false);
        if (comment != null)
        {
            comment.SetDeleted(userId);
            await Context.SaveChangesAsync(token).ConfigureAwait(false);
        }
    }

    #region IDisposable, IAsyncDisposable

    private bool _disposed;
    ~CommentRepository() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(obj: this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(obj: this);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // ReSharper disable once UnusedParameter.Global
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        Context.Dispose();
        _disposed = true;
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual ValueTask DisposeAsyncCore() => Context.DisposeAsync();

    #endregion
}

[SuppressMessage("", "S2436")]
public abstract class CommentRepository<TEntity, TComment, TContext>(TContext context)
    : CommentRepository<TEntity, int, TComment, TContext>(context), ICommentRepository<TComment>
    where TEntity : class, IEntity<int>, IComments<TComment>
    where TComment : Comment
    where TContext : DbContext;
