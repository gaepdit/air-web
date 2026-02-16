using AirWeb.Domain.Compliance.DataExchange;

namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

// Informal Enforcement Actions include:
// * Notices of Violation
// * Combined NOV/NFAs
// * Proposed Consent Orders

public interface IInformalEnforcementAction : IDataExchange;
