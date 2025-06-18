using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.Domain.ComplianceEntities.Fces;
using ClosedXML.Attributes;

namespace AirWeb.AppServices.Compliance.Fces.Search;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record FceExportDto : ISearchResult
{
    public FceExportDto(Fce fce)
    {
        FceId = fce.Id;
        FacilityId = fce.FacilityId;
        Year = fce.Year;
        ReviewedBy = fce.ReviewedBy?.SortableFullName;
        CompletedDate = fce.CompletedDate;
        Notes = fce.Notes;
        Deleted = fce.IsDeleted ? "Deleted" : "No";
    }

    [XLColumn(Header = "FCE ID")]
    public int FceId { get; init; }

    [XLColumn(Header = "Facility ID")]
    public string FacilityId { get; init; }

    [XLColumn(Header = "Facility")]
    public string? FacilityName { get; set; }

    [XLColumn(Header = "FCE Year")]
    public int Year { get; init; }

    [XLColumn(Header = "Reviewed By")]
    public string? ReviewedBy { get; init; }

    [XLColumn(Header = "Date Completed")]
    public DateOnly? CompletedDate { get; init; }

    [XLColumn(Header = "Notes")]
    public string? Notes { get; init; }

    [XLColumn(Header = "Deleted?")]
    public string Deleted { get; init; }
}
