using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

internal static class ComplianceSearchFilters
{
    public static Expression<Func<WorkEntry, bool>> SearchPredicate(ComplianceSearchDto spec) =>
        PredicateBuilder.True<WorkEntry>()
            .ByDeletedStatus(spec.DeletedStatus)
            .FromClosedDate(spec.ReceivedFrom)
            .ToClosedDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .ContainsText(spec.Text);

    private static Expression<Func<WorkEntry, bool>> ByDeletedStatus(
        this Expression<Func<WorkEntry, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
        _ => predicate.And(workEntry => !workEntry.IsDeleted),
    };

    private static Expression<Func<WorkEntry, bool>> FromClosedDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate >= input.Value);

    private static Expression<Func<WorkEntry, bool>> ToClosedDate(
        this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate <= input.Value);

    private static Expression<Func<WorkEntry, bool>> ReceivedBy(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == input);

    private static Expression<Func<WorkEntry, bool>> ContainsText(
        this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
