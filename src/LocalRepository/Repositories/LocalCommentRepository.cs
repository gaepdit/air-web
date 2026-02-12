using AirWeb.Core.Entities;
using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.LocalRepository.Repositories;

[SuppressMessage("", "S2436")]
public abstract class LocalCommentRepository<TEntity, TKey, TComment>(IEnumerable<TEntity> items)
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

    ~LocalCommentRepository() => Dispose(disposing: false);

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

public abstract class LocalCommentRepository<TEntity, TComment>(IEnumerable<TEntity> items)
    : LocalCommentRepository<TEntity, int, TComment>(items), ICommentRepository<TComment>
    where TEntity : class, IEntity<int>, IComments<TComment>
    where TComment : Comment;

public class LocalCaseFileCommentRepository()
    : LocalCommentRepository<CaseFile, CaseFileComment>(CaseFileData.GetData), ICaseFileCommentRepository;

public class LocalComplianceWorkCommentRepository()
    : LocalCommentRepository<ComplianceWork, ComplianceWorkComment>(ComplianceWorkData.GetData),
        IComplianceWorkCommentRepository;

public class LocalFceCommentRepository()
    : LocalCommentRepository<Fce, FceComment>(FceData.GetData), IFceCommentRepository;
