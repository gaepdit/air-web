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
    {
        Id = id;
        CaseFile = caseFile;
        SetCreator(user?.Id);
    }

    // Properties
    public ushort? ActionNumber { get; init; }
    public DataExchangeStatus DataExchangeStatus { get; init; }
    public DateTimeOffset DataExchangeStatusDate { get; init; }
    public bool DataExchangeExempt { get; init; }
}
