using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Core.Entities;

namespace AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;

public class NovNfaLetter : DxActionEnforcementAction, IInformalEnforcementAction, IResponseRequested
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private NovNfaLetter() { }

    internal NovNfaLetter(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.NovNfaLetter;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }

    [StringLength(7000)]
    public string? ResponseComment { get; set; }
}
