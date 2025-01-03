﻿using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class EnforcementLetter : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementLetter() { }

    internal EnforcementLetter(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.EnforcementLetter;
    }

    public bool ResponseRequested { get; set; }
    public DateOnly? ResponseReceived { get; set; }
}
