using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class AdministrativeOrder : ReportableEnforcement, IFormalEnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private AdministrativeOrder() { }

    internal AdministrativeOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.AdministrativeOrder;
    }

    public DateOnly? ExecutedDate { get; set; }
    public bool IsExecuted => ExecutedDate.HasValue;
    public void Execute(DateOnly executedDate) => ExecutedDate = executedDate;

    public DateOnly? AppealedDate { get; set; }
    public bool IsAppealed => AppealedDate.HasValue;
    public void Appeal(DateOnly appealDate) => AppealedDate = appealDate;

    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public void Resolve(DateOnly resolvedDate) => ResolvedDate = resolvedDate;
}
