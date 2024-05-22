using AirWeb.Domain.Entities.WorkEntries;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.WorkEntries.Search;

internal static class WorkEntryFilters
{
    public static Expression<Func<BaseWorkEntry, bool>> SearchPredicate(WorkEntrySearchDto spec) =>
        PredicateBuilder.True<BaseWorkEntry>()
            .ByStatus(spec.Status)
            .ByDeletedStatus(spec.DeletedStatus)
            .FromReceivedDate(spec.ReceivedFrom)
            .ToReceivedDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .IsEntryType(spec.EntryType)
            .ContainsText(spec.Text);

    private static Expression<Func<BaseWorkEntry, bool>> IsClosed(this Expression<Func<BaseWorkEntry, bool>> predicate) =>
        predicate.And(entry => entry.IsClosed);

    private static Expression<Func<BaseWorkEntry, bool>> IsOpen(this Expression<Func<BaseWorkEntry, bool>> predicate) =>
        predicate.And(workEntry => !workEntry.IsClosed);

    private static Expression<Func<BaseWorkEntry, bool>> ByStatus(this Expression<Func<BaseWorkEntry, bool>> predicate,
        WorkEntryStatus? input) => input switch
    {
        WorkEntryStatus.Open => predicate.IsOpen(),
        WorkEntryStatus.Closed => predicate.IsClosed(),
        _ => predicate,
    };

    private static Expression<Func<BaseWorkEntry, bool>> ByDeletedStatus(this Expression<Func<BaseWorkEntry, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
        _ => predicate.And(workEntry => !workEntry.IsDeleted),
    };

    private static Expression<Func<BaseWorkEntry, bool>> FromReceivedDate(this Expression<Func<BaseWorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ReceivedDate.Date >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<BaseWorkEntry, bool>> ToReceivedDate(this Expression<Func<BaseWorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ReceivedDate.Date <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<BaseWorkEntry, bool>> ReceivedBy(this Expression<Func<BaseWorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ReceivedBy != null && entry.ReceivedBy.Id == input);

    private static Expression<Func<BaseWorkEntry, bool>> IsEntryType(this Expression<Func<BaseWorkEntry, bool>> predicate,
        Guid? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.EntryType != null && entry.EntryType.Id == input);

    private static Expression<Func<BaseWorkEntry, bool>> ContainsText(this Expression<Func<BaseWorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
