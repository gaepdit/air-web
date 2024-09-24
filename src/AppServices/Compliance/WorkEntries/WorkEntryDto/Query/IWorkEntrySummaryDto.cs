using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public interface IWorkEntrySummaryDto : ICloseableAndDeletable
{
    public string ItemName { get; }
    public FacilityViewDto Facility { get; set; }
    public string FacilityId { get; }
    public WorkEntryType WorkEntryType { get; }
    public StaffViewDto? ResponsibleStaff { get; }

    // Properties: Closure
    public StaffViewDto? ClosedBy { get; }
    public DateOnly? ClosedDate { get; }
}
