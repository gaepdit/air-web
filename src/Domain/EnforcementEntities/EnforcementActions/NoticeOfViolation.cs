using AirWeb.Core.Entities;
using AirWeb.Domain.EnforcementEntities.CaseFiles;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class NoticeOfViolation : DxActionEnforcementAction, IInformalEnforcementAction, IResponseRequested
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NoticeOfViolation() { }

    internal NoticeOfViolation(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.NoticeOfViolation;
    }

    public bool ResponseRequested { get; set; } = true;
    public DateOnly? ResponseReceived { get; set; }

    [StringLength(7000)]
    public string? ResponseComment { get; set; }
}
