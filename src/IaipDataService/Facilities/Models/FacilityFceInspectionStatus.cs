using ClosedXML.Attributes;
using IaipDataService.Utilities;

namespace IaipDataService.Facilities.Models;

public record FacilityFceInspectionStatus
{
    [XLColumn(Header = "Facility ID")]
    public FacilityId Id { get; init; } = null!;

    [XLColumn(Header = "Facility Name")]
    public string Name { get; init; } = null!;

    [XLColumn(Header = "City")]
    public string City { get; init; } = null!;

    [XLColumn(Ignore = true)]
    public FacilityOperatingStatus OperatingStatusCode { get; init; }

    [XLColumn(Header = "Operating Status")]
    public string OperatingStatus => OperatingStatusCode.GetDisplayName();

    [XLColumn(Ignore = true)]
    public FacilityClassification ClassificationCode { get; init; }

    [XLColumn(Header = "Classification")]
    public string Classification => ClassificationCode.GetDisplayName();

    [XLColumn(Ignore = true)]
    public FacilityCmsClassification CmsClassificationCode { get; init; }

    [XLColumn(Header = "CMS Classification")]
    public string CmsClassification => CmsClassificationCode.GetDisplayName();

    [XLColumn(Header = "Most Recent FCE")]
    public DateOnly? MostRecentFce { get; init; }

    [XLColumn(Header = "Most Recent Inspection")]
    public DateOnly? MostRecentInspectionDate { get; init; }

    [XLColumn(Header = "IAIP Compliance Assignment")]
    public string IaipComplianceAssignment { get; init; } = null!;

    [XLColumn(Header = "IAIP Assignment Year")]
    public int IaipAssignmentYear { get; init; }

    [XLColumn(Header = "Compliance Unit")]
    public string ComplianceUnit { get; init; } = null!;

    [XLColumn(Header = "District")]
    public string District { get; init; } = null!;

    [XLColumn(Ignore = true)]
    public bool IsDistrictResponsible { get; init; }

    [XLColumn(Header = "Responsible Office")]
    public string ResponsibleOffice => IsDistrictResponsible ? "District Office" : "Air Branch";
}
