using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public abstract class ReportableEnforcementAction : EnforcementAction, IDataExchange
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected ReportableEnforcementAction() { }

    private protected ReportableEnforcementAction(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user) { }

    // Properties
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
    public bool DataExchangeExempt { get; init; }
}
