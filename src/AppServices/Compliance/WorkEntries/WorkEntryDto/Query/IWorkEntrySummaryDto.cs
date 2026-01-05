using AirWeb.AppServices.DtoInterfaces;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public interface IWorkEntrySummaryDto : IIsClosed, IHasOwner, IDeleteComments, IDeletable
{
    public string ItemName { get; }
    public string FacilityId { get; }
    public string? FacilityName { get; set; }
    public WorkEntryType WorkEntryType { get; }
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
