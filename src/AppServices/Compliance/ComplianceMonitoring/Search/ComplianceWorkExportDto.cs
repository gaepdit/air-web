using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using ClosedXML.Attributes;
using GaEpd.AppLibrary.Extensions;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ComplianceWorkExportDto : ISearchResult
{
    public ComplianceWorkExportDto(ComplianceWork complianceWork)
    {
        ComplianceWorkId = complianceWork.Id;
        FacilityId = complianceWork.FacilityId;
        ComplianceWorkType = complianceWork.ComplianceWorkType.GetDisplayName();
        EventDate = complianceWork.EventDate;
        EventDateName = complianceWork.EventDateName;
        ResponsibleStaff = complianceWork.ResponsibleStaff?.SortableFullName;
        Closed = complianceWork.IsClosed ? "Closed" : "Open";
        ClosedDate = complianceWork.ClosedDate;
        Notes = complianceWork.Notes;
        Deleted = complianceWork.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "Compliance Work ID")]
    public int ComplianceWorkId { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string FacilityId { get; init; }

    [XLColumn(Header = "Facility")]
    public string? FacilityName { get; set; }

    [XLColumn(Header = "Compliance Work Type")]
    public string ComplianceWorkType { get; init; }

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
