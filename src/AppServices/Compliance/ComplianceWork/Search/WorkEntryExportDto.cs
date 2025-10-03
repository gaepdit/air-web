using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Search;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record WorkEntryExportDto : ISearchResult
{
    public WorkEntryExportDto(WorkEntry workEntry)
    {
        WorkEntryId = workEntry.Id;
        FacilityId = workEntry.FacilityId;
        WorkEntryType = workEntry.WorkEntryType.GetDisplayName();
        EventDate = workEntry.EventDate;
        EventDateName = workEntry.EventDateName;
        ResponsibleStaff = workEntry.ResponsibleStaff?.SortableFullName;
        Closed = workEntry.IsClosed ? "Closed" : "Open";
        ClosedDate = workEntry.ClosedDate;
        Notes = workEntry.Notes;
        Deleted = workEntry.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Work Entry ID")]
    public int WorkEntryId { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string FacilityId { get; init; }

    [XLColumn(Header = "Facility")]
    public string? FacilityName { get; set; }

    [XLColumn(Header = "Work Entry Type")]
    public string WorkEntryType { get; init; }

    [XLColumn(Header = "Date Description")]
    public DateOnly EventDate { get; init; }

    [XLColumn(Header = "Event")]
    public string EventDateName { get; init; }

    [XLColumn(Header = "Staff Responsible")]
    public string? ResponsibleStaff { get; init; }

    [XLColumn(Header = "Closed?")]
    public string Closed { get; init; }

    [XLColumn(Header = "Date Closed")]
    public DateOnly? ClosedDate { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
