using GaEpd.AppLibrary.Domain.Predicates;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.WorkEntries;
using System.Linq.Expressions;

namespace MyApp.AppServices.WorkEntries;

internal static class WorkEntryFilters
{
    public static Expression<Func<WorkEntry, bool>> SearchPredicate(WorkEntrySearchDto spec) =>
        PredicateBuilder.True<WorkEntry>()
            .ByStatus(spec.Status)
            .ByDeletedStatus(spec.DeletedStatus)
            .FromReceivedDate(spec.ReceivedFrom)
            .ToReceivedDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .IsEntryType(spec.EntryType)
            .ContainsText(spec.Text);

    private static Expression<Func<WorkEntry, bool>> IsClosed(this Expression<Func<WorkEntry, bool>> predicate) =>
        predicate.And(entry => entry.Closed);

    private static Expression<Func<WorkEntry, bool>> IsOpen(this Expression<Func<WorkEntry, bool>> predicate) =>
        predicate.And(workEntry => !workEntry.Closed);

    private static Expression<Func<WorkEntry, bool>> ByStatus(this Expression<Func<WorkEntry, bool>> predicate,
        WorkEntryStatus? input) => input switch
    {
        WorkEntryStatus.Open => predicate.IsOpen(),
        WorkEntryStatus.Closed => predicate.IsClosed(),
        _ => predicate,
    };

    private static Expression<Func<WorkEntry, bool>> ByDeletedStatus(this Expression<Func<WorkEntry, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
        _ => predicate.And(workEntry => !workEntry.IsDeleted),
    };

    private static Expression<Func<WorkEntry, bool>> FromReceivedDate(this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ReceivedDate.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<WorkEntry, bool>> ToReceivedDate(this Expression<Func<WorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ReceivedDate.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<WorkEntry, bool>> ReceivedBy(this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ReceivedBy != null && entry.ReceivedBy.Id == input);

    private static Expression<Func<WorkEntry, bool>> IsEntryType(this Expression<Func<WorkEntry, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.EntryType != null && entry.EntryType.Id == input);

    private static Expression<Func<WorkEntry, bool>> ContainsText(this Expression<Func<WorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
