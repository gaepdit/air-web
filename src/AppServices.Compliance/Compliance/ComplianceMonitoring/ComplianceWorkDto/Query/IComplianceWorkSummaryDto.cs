using AirWeb.AppServices.Compliance.DtoInterfaces;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Core.BaseEntities;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;

public interface IComplianceWorkSummaryDto : IIsClosed, IHasOwner, IDeleteComments, IDeletable
{
    public string ItemName { get; }
    public string FacilityId { get; }
    public string? FacilityName { get; set; }
    public ComplianceWorkType ComplianceWorkType { get; }
    public bool IsComplianceEvent { get; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; }

    public DateOnly EventDate { get; }
    public string EventDateName { get; }

    // Properties: Closure
    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; }
}
