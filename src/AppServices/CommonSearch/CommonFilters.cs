using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.BaseEntities.Interfaces;
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
            DeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
            _ => predicate.And(entry => !entry.IsDeleted),
        };

    public static Expression<Func<TEntity, bool>> ByClosedStatus<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        ClosedOpenAny? input) where TEntity : IIsClosed =>
        input switch
        {
            ClosedOpenAny.Closed => predicate.And(entry => entry.IsClosed),
            ClosedOpenAny.Open => predicate.And(entry => !entry.IsClosed),
            _ => predicate,
        };

    public static Expression<Func<TEntity, bool>> ByFacilityId<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : IFacilityId =>
        string.IsNullOrWhiteSpace(input) || !FacilityId.IsValidFormat(input)
            ? predicate
            : predicate.And(entry => entry.FacilityId == new FacilityId(input).FormattedId);

    public static Expression<Func<TEntity, bool>> ByNotesText<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : INotes =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.Notes != null && entry.Notes.Contains(input));
}
