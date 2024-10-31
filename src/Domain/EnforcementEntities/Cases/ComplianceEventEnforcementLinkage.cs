using AirWeb.Domain.ComplianceEntities.WorkEntries;

namespace AirWeb.Domain.EnforcementEntities.Cases;

public record ComplianceEventEnforcementLinkage
{
    public EnforcementCase EnforcementCase { get; set; } = null!;
    public ComplianceEvent ComplianceEvent { get; set; } = null!;
}
