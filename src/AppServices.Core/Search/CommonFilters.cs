using AirWeb.Core.BaseEntities;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Core.Search;

public static class CommonFilters
{
    public static Expression<Func<TEntity, bool>> ByDeletedStatus<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        DeleteStatus? input) where TEntity : IIsDeleted =>
        input switch
        {
            DeleteStatus.All => predicate,
            DeleteStatus.Deleted => predicate.And(entity => entity.IsDeleted),
            _ => predicate.And(entity => !entity.IsDeleted),
        };

    public static Expression<Func<TEntity, bool>> ByClosedStatus<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        ClosedOpenAny? input) where TEntity : IIsClosed =>
        input switch
        {
            ClosedOpenAny.Closed => predicate.And(entity => entity.IsClosed),
            ClosedOpenAny.Open => predicate.And(entity => !entity.IsClosed),
            _ => predicate,
        };

    public static Expression<Func<TEntity, bool>> ByNotesText<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : INotes =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entity => entity.Notes != null && entity.Notes.Contains(input));
}
