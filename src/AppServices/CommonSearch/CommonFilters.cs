using AirWeb.Core.BaseEntities;
using AirWeb.Domain.CommonInterfaces;
using GaEpd.AppLibrary.Domain.Predicates;
using IaipDataService.Facilities;
using System.Linq.Expressions;

namespace AirWeb.AppServices.CommonSearch;

internal static class CommonFilters
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

    public static Expression<Func<TEntity, bool>> ByFacilityId<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : IFacilityId
    {
        if (string.IsNullOrWhiteSpace(input))
            return predicate;

        if (!FacilityId.IsValidFormat(input))
            return PredicateBuilder.False<TEntity>();

        return predicate.And(entity => entity.FacilityId == new FacilityId(input).FormattedId);
    }

    public static Expression<Func<TEntity, bool>> ByNotesText<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : INotes =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entity => entity.Notes != null && entity.Notes.Contains(input));
}
