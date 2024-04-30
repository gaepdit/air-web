using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;
using MyApp.Domain.Entities.WorkEntries;

namespace MyApp.AppServices.DataExport;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record SearchResultsExportDto
{
    public SearchResultsExportDto(WorkEntry workEntry)
    {
        WorkEntryId = workEntry.Id;
        ReceivedDate = workEntry.ReceivedDate;
        ReceivedByName = workEntry.ReceivedBy?.SortableFullName;
        Status = workEntry.Status.GetDisplayName();
        EntryType = workEntry.EntryType?.Name;
        DateClosed = workEntry.ClosedDate;
        Notes = workEntry.Notes;
        Deleted = workEntry.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Work Entry ID")]
    public Guid WorkEntryId { get; init; }

    [XLColumn(Header = "Date Received")]
    public DateTimeOffset ReceivedDate { get; init; }

    [XLColumn(Header = "Received By")]
    public string? ReceivedByName { get; init; }

    [XLColumn(Header = "Status")]
    public string Status { get; init; }

    [XLColumn(Header = "Entry Type")]
    public string? EntryType { get; init; }

    [XLColumn(Header = "Date Closed")]
    public DateTimeOffset? DateClosed { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
