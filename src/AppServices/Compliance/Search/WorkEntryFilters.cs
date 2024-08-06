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
            .ByOffice(spec.Offices)
            .ByResponsibleStaff(spec.ResponsibleStaff)
            .FromEventDate(spec.EventDateFrom)
            .ToEventDate(spec.EventDateTo)
            .FromClosedDate(spec.ClosedDateFrom)
            .ToClosedDate(spec.ClosedDateTo)
            .ByNotesText(spec.Notes);

    private static Expression<Func<WorkEntry, bool>> ByDeletedStatus(
        this Expression<Func<WorkEntry, bool>> predicate,
        DeleteStatus? input) => input switch
    {
        DeleteStatus.All => predicate,
        DeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
        _ => predicate.And(entry => !entry.IsDeleted),
    };

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
        List<WorkEntryTypes> input)
    {
        if (input.Count == 0) return predicate;

        var includePredicate = PredicateBuilder.False<WorkEntry>();

        if (input.Contains(WorkEntryTypes.Acc))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.AnnualComplianceCertification);

        if (input.Contains(WorkEntryTypes.Inspection))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.Inspection);

        if (input.Contains(WorkEntryTypes.Rmp))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.RmpInspection);

        if (input.Contains(WorkEntryTypes.Report))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.Report);

        if (input.Contains(WorkEntryTypes.Str))
            includePredicate = includePredicate.Or(entry =>
                entry.WorkEntryType == WorkEntryType.ComplianceEvent &&
                ((ComplianceEvent)entry).ComplianceEventType == ComplianceEventType.SourceTestReview);

        if (input.Contains(WorkEntryTypes.Notification))
            includePredicate = includePredicate.Or(entry => entry.WorkEntryType == WorkEntryType.Notification);

        if (input.Contains(WorkEntryTypes.PermitRevocation))
            includePredicate = includePredicate.Or(entry => entry.WorkEntryType == WorkEntryType.PermitRevocation);

        return predicate.And(includePredicate);
    }

    private static Expression<Func<WorkEntry, bool>> ByFacilityId(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(fce => fce.FacilityId.Contains(input));

    private static Expression<Func<WorkEntry, bool>> ByResponsibleStaff(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == input);

    private static Expression<Func<WorkEntry, bool>> ByOffice(
        this Expression<Func<WorkEntry, bool>> predicate,
        List<Guid> input) =>
        input.Count == 0
            ? predicate
            : predicate.And(entry =>
                entry.ResponsibleStaff != null &&
                entry.ResponsibleStaff.Office != null &&
                input.Contains(entry.ResponsibleStaff.Office.Id));

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

    private static Expression<Func<WorkEntry, bool>> ByNotesText(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
