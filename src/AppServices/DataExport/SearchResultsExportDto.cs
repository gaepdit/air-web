using AirWeb.Domain.Entities.WorkEntries;
using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.DataExport;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record SearchResultsExportDto
{
    public SearchResultsExportDto(BaseWorkEntry baseWorkEntry)
    {
        WorkEntryId = baseWorkEntry.Id;
        WorkEntryType = baseWorkEntry.WorkEntryType.GetDescription();
        FacilityId = baseWorkEntry.Facility.Id;
        Facility = baseWorkEntry.Facility.CompanyName;
        ResponsibleStaff = baseWorkEntry.ResponsibleStaff?.SortableFullName;
        DateClosed = baseWorkEntry.ClosedDate;
        Notes = baseWorkEntry.Notes;
        Deleted = baseWorkEntry.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Work Entry ID")]
    public int WorkEntryId { get; init; }

    [XLColumn(Header = "Work Entry Type")]
    public string WorkEntryType { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string FacilityId { get; init; }

    [XLColumn(Header = "Facility")]
    public string Facility { get; init; }

    [XLColumn(Header = "Staff Responsible")]
    public string? ResponsibleStaff { get; init; }

    [XLColumn(Header = "Date Closed")]
    public DateTimeOffset? DateClosed { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
