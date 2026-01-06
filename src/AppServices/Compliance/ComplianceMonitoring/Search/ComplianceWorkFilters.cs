using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;

internal static class ComplianceWorkFilters
{
    public static Expression<Func<ComplianceWork, bool>> SearchPredicate(ComplianceWorkSearchDto spec) =>
        PredicateBuilder.True<ComplianceWork>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByClosedStatus(spec.Closed)
            .ByWorkType(spec.Include)
            .ByFacilityId(spec.FacilityId)
            .ByOffice(spec.Office)
            .ByStaff(spec.Staff)
            .MinEventDate(spec.EventDateFrom)
            .MaxEventDate(spec.EventDateTo)
            .MinClosedDate(spec.ClosedDateFrom)
            .MaxClosedDate(spec.ClosedDateTo)
            .ByNotesText(spec.Notes);

    public static Expression<Func<ComplianceWork, bool>> ByWorkType(
        this Expression<Func<ComplianceWork, bool>> predicate,
        List<WorkTypeSearch> input)
    {
        if (input.Count == 0) return predicate;

        var includePredicate = PredicateBuilder.False<ComplianceWork>();

        if (input.Contains(WorkTypeSearch.Acc))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.AnnualComplianceCertification);

        if (input.Contains(WorkTypeSearch.Inspection))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.Inspection);

        if (input.Contains(WorkTypeSearch.Notification))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.Notification);

        if (input.Contains(WorkTypeSearch.PermitRevocation))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.PermitRevocation);

        if (input.Contains(WorkTypeSearch.Report))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.Report);

        if (input.Contains(WorkTypeSearch.Rmp))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.RmpInspection);

        if (input.Contains(WorkTypeSearch.Str))
            includePredicate = includePredicate
                .Or(work => work.ComplianceWorkType == ComplianceWorkType.SourceTestReview);

        return predicate.And(includePredicate);
    }

    public static Expression<Func<ComplianceWork, bool>> ByStaff(
        this Expression<Func<ComplianceWork, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(work => work.ResponsibleStaff != null && work.ResponsibleStaff.Id == input);

    public static Expression<Func<ComplianceWork, bool>> ByOffice(
        this Expression<Func<ComplianceWork, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(work =>
                work.ResponsibleStaff != null &&
                work.ResponsibleStaff.Office != null &&
                work.ResponsibleStaff.Office.Id == input);

    public static Expression<Func<ComplianceWork, bool>> MinEventDate(
        this Expression<Func<ComplianceWork, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(work => work.EventDate >= input);

    public static Expression<Func<ComplianceWork, bool>> MaxEventDate(
        this Expression<Func<ComplianceWork, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(work => work.EventDate <= input);

    public static Expression<Func<ComplianceWork, bool>> MinClosedDate(
        this Expression<Func<ComplianceWork, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(work => work.ClosedDate >= input);

    public static Expression<Func<ComplianceWork, bool>> MaxClosedDate(
        this Expression<Func<ComplianceWork, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(work => work.ClosedDate <= input);
}
