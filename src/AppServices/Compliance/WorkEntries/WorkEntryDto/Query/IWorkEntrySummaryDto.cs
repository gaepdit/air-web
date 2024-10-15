using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using IaipDataService.Facilities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public interface IWorkEntrySummaryDto : ICloseableAndDeletable, IHasOwnerAndDeletable
{
    public string ItemName { get; }
    public Facility Facility { get; set; }
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }

    [Display(Name = "Staff Responsible")]
    public StaffViewDto? ResponsibleStaff { get; }

    // Properties: Closure
    [Display(Name = "Completed By")]
    public StaffViewDto? ClosedBy { get; }

    [Display(Name = "Date Closed")]
    public DateOnly? ClosedDate { get; }
}
