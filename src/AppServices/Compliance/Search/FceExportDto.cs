using AirWeb.Domain.ComplianceEntities.Fces;
using ClosedXML.Attributes;

namespace AirWeb.AppServices.Compliance.Search;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record FceExportDto : IExportDto
{
    public FceExportDto(Fce fce)
    {
        FceId = fce.Id;
        FacilityId = fce.Facility.Id;
        Facility = fce.Facility.Name;
        Year = fce.Year;
        ReviewedBy = fce.ReviewedBy?.SortableFullName;
        DateCompleted = fce.CompletedDate;
        Notes = fce.Notes;
        Deleted = fce.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "FCE ID")]
    public int FceId { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string FacilityId { get; init; }

    [XLColumn(Header = "Facility")]
    public string Facility { get; init; }

    [XLColumn(Header = "FCE Year")]
    public int Year { get; init; }

    [XLColumn(Header = "Reviewed By")]
    public string? ReviewedBy { get; init; }

    [XLColumn(Header = "Date Completed")]
    public DateOnly? DateCompleted { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
