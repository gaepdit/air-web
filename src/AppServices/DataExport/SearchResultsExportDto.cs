using AirWeb.Domain.ComplianceEntities.WorkEntries;
using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.DataExport;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record SearchResultsExportDto
{
    public SearchResultsExportDto(WorkEntry workEntry)
    {
        WorkEntryId = workEntry.Id;
        WorkEntryType = workEntry.WorkEntryType.GetDescription();
        FacilityId = workEntry.Facility.Id;
        Facility = workEntry.Facility.Name;
        ResponsibleStaff = workEntry.ResponsibleStaff?.SortableFullName;
        DateClosed = workEntry.ClosedDate;
        Notes = workEntry.Notes;
        Deleted = workEntry.IsDeleted ? "Deleted" : "No";
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
    public DateOnly? DateClosed { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
