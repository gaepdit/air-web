using AirWeb.AppServices.Core.Search;
using AirWeb.AppServices.FacilitySearch;
using AirWeb.Domain.ComplianceEntities.Fces;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Fces.Search;

internal static class FceFilters
{
    public static Expression<Func<Fce, bool>> SearchPredicate(FceSearchDto spec) =>
        PredicateBuilder.True<Fce>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByFacilityId(spec.FacilityId)
            .ByYear(spec.Year)
            .ByStaff(spec.ReviewedBy)
            .ByOffice(spec.Office)
            .MinCompletedDate(spec.DateFrom)
            .MaxCompletedDate(spec.DateTo)
            .ByOnsiteStatus(spec.Onsite)
            .ByNotesText(spec.Notes);

    public static Expression<Func<Fce, bool>> ByYear(
        this Expression<Func<Fce, bool>> predicate,
        int? input) =>
        input is null ? predicate : predicate.And(fce => fce.Year.Equals(input));

    public static Expression<Func<Fce, bool>> ByStaff(
        this Expression<Func<Fce, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(fce => fce.ReviewedBy != null && fce.ReviewedBy.Id == input);

    public static Expression<Func<Fce, bool>> ByOffice(
        this Expression<Func<Fce, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(fce =>
                fce.ReviewedBy != null &&
                fce.ReviewedBy.Office != null &&
                fce.ReviewedBy.Office.Id == input);

    public static Expression<Func<Fce, bool>> MinCompletedDate(
        this Expression<Func<Fce, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(fce => fce.CompletedDate >= input);

    public static Expression<Func<Fce, bool>> MaxCompletedDate(
        this Expression<Func<Fce, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(fce => fce.CompletedDate <= input);

    public static Expression<Func<Fce, bool>> ByOnsiteStatus(
        this Expression<Func<Fce, bool>> predicate,
        YesNoAny? input) =>
        input switch
        {
            YesNoAny.Yes => predicate.And(fce => fce.OnsiteInspection),
            YesNoAny.No => predicate.And(fce => !fce.OnsiteInspection),
            _ => predicate,
        };
}
