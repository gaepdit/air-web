using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Core.Entities;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.MemRepository.Repositories;

[SuppressMessage("", "S2436")]
public abstract class CommentMemRepository<TEntity, TKey, TComment>(IEnumerable<TEntity> items)
    : ICommentRepository<TKey, TComment>
    where TEntity : class, IEntity<TKey>, IComments<TComment>
    where TKey : IEquatable<TKey>
    where TComment : Comment
{
    private IEnumerable<TEntity> Items { get; } = items;

    public Task AddCommentAsync(TKey itemId, TComment comment, CancellationToken token = default)
    {
        var entity = Items.SingleOrDefault(entity => entity.Id.Equals(itemId)) ??
                     throw new EntityNotFoundException<TEntity>(itemId);
        entity.Comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteCommentAsync(Guid commentId, string? userId, CancellationToken token = default)
    {
        var comment = Items.SelectMany(entity => entity.Comments).FirstOrDefault(comment => comment.Id == commentId);
        if (comment == null) return Task.CompletedTask;

        var entity = Items.First(entity => entity.Comments.Contains(comment));
        entity.Comments.Remove(comment);
        return Task.CompletedTask;
    }

    #region IDisposable,  IAsyncDisposable

    ~CommentMemRepository() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(obj: this);
    }

    public ValueTask DisposeAsync()
    {
        Dispose(disposing: false);
        GC.SuppressFinalize(obj: this);
        return ValueTask.CompletedTask;
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // ReSharper disable once UnusedParameter.Global
    protected virtual void Dispose(bool disposing) { }

    #endregion
}

public abstract class CommentMemRepository<TEntity, TComment>(IEnumerable<TEntity> items)
    : CommentMemRepository<TEntity, int, TComment>(items), ICommentRepository<TComment>
    where TEntity : class, IEntity<int>, IComments<TComment>
    where TComment : Comment;

public class CaseFileCommentMemRepository()
    : CommentMemRepository<CaseFile, CaseFileComment>(CaseFileData.GetData), ICaseFileCommentRepository;

public class ComplianceWorkCommentMemRepository()
    : CommentMemRepository<ComplianceWork, ComplianceWorkComment>(ComplianceWorkData.GetData),
        IComplianceWorkCommentRepository;

public class FceCommentMemRepository()
    : CommentMemRepository<Fce, FceComment>(FceData.GetData), IFceCommentRepository;
