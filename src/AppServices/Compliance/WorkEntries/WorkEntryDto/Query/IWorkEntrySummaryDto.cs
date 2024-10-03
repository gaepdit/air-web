using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public interface IWorkEntrySummaryDto : ICloseableAndDeletable
{
    public string ItemName { get; }
    public Facility Facility { get; set; }
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }
    public StaffViewDto? ResponsibleStaff { get; }

    // Properties: Closure
    public StaffViewDto? ClosedBy { get; }
    public DateOnly? ClosedDate { get; }
}
