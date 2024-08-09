using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

internal static class WorkEntryFilters
{
    public static Expression<Func<WorkEntry, bool>> SearchPredicate(WorkEntrySearchDto spec) =>
        PredicateBuilder.True<WorkEntry>()
            .ByDeletedStatus(spec.DeleteStatus)
            .ByClosedStatus(spec.Closed)
            .ByWorkType(spec.Include)
            .ByFacilityId(spec.PartialFacilityId)
            .ByOffice(spec.Office)
            .ByResponsibleStaff(spec.ResponsibleStaff)
            .FromEventDate(spec.EventDateFrom)
            .ToEventDate(spec.EventDateTo)
            .FromClosedDate(spec.ClosedDateFrom)
            .ToClosedDate(spec.ClosedDateTo)
            .ByNotesText(spec.Notes);

    private static Expression<Func<WorkEntry, bool>> ByClosedStatus(
        this Expression<Func<WorkEntry, bool>> predicate,
        YesNoAny? input) => input switch
    {
        YesNoAny.Yes => predicate.And(entry => entry.IsClosed),
        YesNoAny.No => predicate.And(entry => !entry.IsClosed),
        _ => predicate,
    };

    private static Expression<Func<WorkEntry, bool>> ByWorkType(
        this Expression<Func<WorkEntry, bool>> predicate,
        List<WorkTypeSearch> input)
    {
        if (input.Count == 0) return predicate;

        var includePredicate = PredicateBuilder.False<WorkEntry>();

        if (input.Contains(WorkTypeSearch.Acc))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.AnnualComplianceCertification);

        if (input.Contains(WorkTypeSearch.Inspection))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.Inspection);

        if (input.Contains(WorkTypeSearch.Rmp))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.RmpInspection);

        if (input.Contains(WorkTypeSearch.Report))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.Report);

        if (input.Contains(WorkTypeSearch.Str))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.SourceTestReview);

        if (input.Contains(WorkTypeSearch.Notification))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.Notification);

        if (input.Contains(WorkTypeSearch.PermitRevocation))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == Domain.ComplianceEntities.WorkEntries.WorkEntryType.PermitRevocation);

        return predicate.And(includePredicate);
    }

    private static Expression<Func<WorkEntry, bool>> ByResponsibleStaff(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == input);

    private static Expression<Func<WorkEntry, bool>> ByOffice(
        this Expression<Func<WorkEntry, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(entry =>
                entry.ResponsibleStaff != null &&
                entry.ResponsibleStaff.Office != null &&
                entry.ResponsibleStaff.Office.Id == input);

    private static Expression<Func<WorkEntry, bool>> FromEventDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.EventDate >= input);

    private static Expression<Func<WorkEntry, bool>> ToEventDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.EventDate <= input);

    private static Expression<Func<WorkEntry, bool>> FromClosedDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate >= input);

    private static Expression<Func<WorkEntry, bool>> ToClosedDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate <= input);
}
