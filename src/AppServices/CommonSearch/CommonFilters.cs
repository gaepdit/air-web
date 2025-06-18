using AirWeb.AppServices.Compliance.Search;
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

    public static Expression<Func<TEntity, bool>> ByFacilityId<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : IFacilityId
    {
        var cleanInput = FacilityId.CleanFacilityId(input);
        return string.IsNullOrWhiteSpace(cleanInput)
            ? predicate
            : predicate.And(entry => entry.FacilityId.Contains(cleanInput));
    }

    public static Expression<Func<TEntity, bool>> ByNotesText<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : INotes =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.Notes != null && entry.Notes.Contains(input));
}
