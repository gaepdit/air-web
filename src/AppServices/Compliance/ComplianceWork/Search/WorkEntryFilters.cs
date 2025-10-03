using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Search;

internal static class WorkEntryFilters
{
    public static Expression<Func<WorkEntry, bool>> SearchPredicate(WorkEntrySearchDto spec) =>
        PredicateBuilder.True<WorkEntry>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByClosedStatus(spec.Closed)
            .ByWorkType(spec.Include)
            .ByFacilityId(spec.PartialFacilityId)
            .ByOffice(spec.Office)
            .ByStaff(spec.Staff)
            .MinEventDate(spec.EventDateFrom)
            .MaxEventDate(spec.EventDateTo)
            .MinClosedDate(spec.ClosedDateFrom)
            .MaxClosedDate(spec.ClosedDateTo)
            .ByNotesText(spec.Notes);

    public static Expression<Func<WorkEntry, bool>> ByWorkType(
        this Expression<Func<WorkEntry, bool>> predicate,
        List<WorkTypeSearch> input)
    {
        if (input.Count == 0) return predicate;

        var includePredicate = PredicateBuilder.False<WorkEntry>();

        if (input.Contains(WorkTypeSearch.Acc))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.AnnualComplianceCertification);

        if (input.Contains(WorkTypeSearch.Inspection))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.Inspection);

        if (input.Contains(WorkTypeSearch.Notification))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.Notification);

        if (input.Contains(WorkTypeSearch.PermitRevocation))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.PermitRevocation);

        if (input.Contains(WorkTypeSearch.Report))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.Report);

        if (input.Contains(WorkTypeSearch.Rmp))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.RmpInspection);

        if (input.Contains(WorkTypeSearch.Str))
            includePredicate = includePredicate
                .Or(entry => entry.WorkEntryType == WorkEntryType.SourceTestReview);

        return predicate.And(includePredicate);
    }

    public static Expression<Func<WorkEntry, bool>> ByStaff(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == input);

    public static Expression<Func<WorkEntry, bool>> ByOffice(
        this Expression<Func<WorkEntry, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(entry =>
                entry.ResponsibleStaff != null &&
                entry.ResponsibleStaff.Office != null &&
                entry.ResponsibleStaff.Office.Id == input);

    public static Expression<Func<WorkEntry, bool>> MinEventDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.EventDate >= input);

    public static Expression<Func<WorkEntry, bool>> MaxEventDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.EventDate <= input);

    public static Expression<Func<WorkEntry, bool>> MinClosedDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate >= input);

    public static Expression<Func<WorkEntry, bool>> MaxClosedDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate <= input);
}
