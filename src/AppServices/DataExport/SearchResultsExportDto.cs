using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;
using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.AppServices.DataExport;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record SearchResultsExportDto
{
    public SearchResultsExportDto(BaseWorkEntry baseWorkEntry)
    {
        WorkEntryId = baseWorkEntry.Id;
        ReceivedDate = baseWorkEntry.ReceivedDate;
        ReceivedByName = baseWorkEntry.ReceivedBy?.SortableFullName;
        Status = baseWorkEntry.Status.GetDisplayName();
        EntryType = baseWorkEntry.EntryType?.Name;
        DateClosed = baseWorkEntry.ClosedDate;
        Notes = baseWorkEntry.Notes;
        Deleted = baseWorkEntry.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Work Entry ID")]
    public int WorkEntryId { get; init; }

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
