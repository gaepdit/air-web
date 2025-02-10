using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.Domain.EnforcementEntities.Actions;

// Informal Enforcement Actions include:
// * Notices of Violation
// * Combined NOV/NFAs
// * Proposed Consent Orders

public interface IInformalEnforcementAction
{
    public CaseFile CaseFile { get; }
}
