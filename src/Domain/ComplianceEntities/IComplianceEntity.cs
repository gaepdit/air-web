using AirWeb.Domain.ExternalEntities.Facilities;

namespace AirWeb.Domain.ComplianceEntities;

public interface IComplianceEntity
{
    public string FacilityId { get; }
    public Facility Facility { get; set; }
    public string Notes { get; }
    public bool IsDeleted { get; }
}
