﻿using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;

namespace AirWeb.Domain.EnforcementEntities.Actions;

public class ProposedConsentOrder : EnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ProposedConsentOrder() { }

    internal ProposedConsentOrder(Guid id, EnforcementCase enforcementCase, ApplicationUser? user) :
        base(id, enforcementCase, user)
    {
        EnforcementActionType = EnforcementActionType.ProposedConsentOrder;
    }

    // Properties
    public DateOnly? ResponseReceived { get; set; }
}
