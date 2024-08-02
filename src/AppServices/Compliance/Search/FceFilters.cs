using AirWeb.Domain.ComplianceEntities.Fces;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

internal static class FceFilters
{
    public static Expression<Func<Fce, bool>> SearchPredicate(FceSearchDto spec) =>
        PredicateBuilder.True<Fce>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByFacilityId(spec.PartialFacilityId)
            .ByYear(spec.Year)
            .ByReviewer(spec.ReviewedBy)
            .ByOffice(spec.Offices)
            .FromDate(spec.DateFrom)
            .ToDate(spec.DateTo)
            .ByOnsiteStatus(spec.Onsite)
            .ByNotesText(spec.Notes);

    private static Expression<Func<Fce, bool>> ByDeletedStatus(
        this Expression<Func<Fce, bool>> predicate,
        DeleteStatus? input) =>
        input switch
        {
            DeleteStatus.All => predicate,
            DeleteStatus.Deleted => predicate.And(fce => fce.IsDeleted),
            _ => predicate.And(fce => !fce.IsDeleted),
        };

    private static Expression<Func<Fce, bool>> ByFacilityId(
        this Expression<Func<Fce, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(fce => fce.FacilityId.Contains(input));

    private static Expression<Func<Fce, bool>> ByYear(
        this Expression<Func<Fce, bool>> predicate,
        int? input) =>
        input is null ? predicate : predicate.And(fce => fce.Year.Equals(input));

    private static Expression<Func<Fce, bool>> ByReviewer(
        this Expression<Func<Fce, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(fce => fce.ReviewedBy != null && fce.ReviewedBy.Id == input);

    private static Expression<Func<Fce, bool>> ByOffice(
        this Expression<Func<Fce, bool>> predicate,
        List<Guid> input) =>
        input.Count == 0
            ? predicate
            : predicate.And(fce =>
                fce.ReviewedBy != null &&
                fce.ReviewedBy.Office != null &&
                input.Contains(fce.ReviewedBy.Office.Id));

    private static Expression<Func<Fce, bool>> FromDate(
        this Expression<Func<Fce, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(fce => fce.CompletedDate >= input);

    private static Expression<Func<Fce, bool>> ToDate(
        this Expression<Func<Fce, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(fce => fce.CompletedDate <= input);

    private static Expression<Func<Fce, bool>> ByOnsiteStatus(
        this Expression<Func<Fce, bool>> predicate,
        YesNoAny? input) =>
        input switch
        {
            YesNoAny.Yes => predicate.And(fce => fce.OnsiteInspection),
            YesNoAny.No => predicate.And(fce => !fce.OnsiteInspection),
            _ => predicate,
        };

    private static Expression<Func<Fce, bool>> ByNotesText(
        this Expression<Func<Fce, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(fce => fce.Notes.Contains(input));
}
