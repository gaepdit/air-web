namespace AirWeb.Domain.ComplianceEntities;

public interface IComplianceEntity
{
    public string FacilityId { get; }
    public string Notes { get; }
    public bool IsDeleted { get; }
}
