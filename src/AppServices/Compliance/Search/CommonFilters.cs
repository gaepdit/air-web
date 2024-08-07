using AirWeb.AppServices.Utilities;
using AirWeb.Domain.ComplianceEntities;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

internal static class CommonFilters
{
    public static Expression<Func<TEntity, bool>> ByDeletedStatus<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        DeleteStatus? input) where TEntity : IComplianceEntity =>
        input switch
        {
            DeleteStatus.All => predicate,
            DeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
            _ => predicate.And(entry => !entry.IsDeleted),
        };

    public static Expression<Func<TEntity, bool>> ByFacilityId<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : IComplianceEntity
    {
        if (string.IsNullOrWhiteSpace(input)) return predicate;
        var cleanInput = input.CleanFacilityId();
        if (string.IsNullOrWhiteSpace(cleanInput)) return predicate;

        // Test for matches with and without hyphen.
        var facilityIdPredicate = PredicateBuilder.False<TEntity>()
            .Or(entry => entry.FacilityId.Contains(input))
            .Or(entry => entry.FacilityId.Contains(cleanInput));
        return predicate.And(facilityIdPredicate);
    }

    public static Expression<Func<TEntity, bool>> ByNotesText<TEntity>(
        this Expression<Func<TEntity, bool>> predicate,
        string? input) where TEntity : IComplianceEntity =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
