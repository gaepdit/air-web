using AirWeb.Domain.Entities.WorkEntries;
using GaEpd.AppLibrary.Domain.Predicates;
using System.Linq.Expressions;

namespace AirWeb.AppServices.WorkEntries.Search;

internal static class WorkEntryFilters
{
    public static Expression<Func<BaseWorkEntry, bool>> SearchPredicate(WorkEntrySearchDto spec) =>
        PredicateBuilder.True<BaseWorkEntry>()
            .ByDeletedStatus(spec.DeletedStatus)
            .FromClosedDate(spec.ReceivedFrom)
            .ToClosedDate(spec.ReceivedTo)
            .ReceivedBy(spec.ReceivedBy)
            .ContainsText(spec.Text);

    private static Expression<Func<BaseWorkEntry, bool>> ByDeletedStatus(this Expression<Func<BaseWorkEntry, bool>> predicate,
        SearchDeleteStatus? input) => input switch
    {
        SearchDeleteStatus.All => predicate,
        SearchDeleteStatus.Deleted => predicate.And(entry => entry.IsDeleted),
        _ => predicate.And(workEntry => !workEntry.IsDeleted),
    };

    private static Expression<Func<BaseWorkEntry, bool>> FromClosedDate(this Expression<Func<BaseWorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate >= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<BaseWorkEntry, bool>> ToClosedDate(this Expression<Func<BaseWorkEntry, bool>> predicate,
        DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(entry => entry.ClosedDate <= input.Value.ToDateTime(TimeOnly.MinValue));

    private static Expression<Func<BaseWorkEntry, bool>> ReceivedBy(this Expression<Func<BaseWorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input)
            ? predicate
            : predicate.And(entry => entry.ResponsibleStaff != null && entry.ResponsibleStaff.Id == input);

    private static Expression<Func<BaseWorkEntry, bool>> ContainsText(this Expression<Func<BaseWorkEntry, bool>> predicate,
        string? input) =>
        string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(entry => entry.Notes.Contains(input));
}
