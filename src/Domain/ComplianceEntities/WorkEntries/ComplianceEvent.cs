﻿using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.Identity;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.ComplianceEntities.WorkEntries;

public abstract class ComplianceEvent : WorkEntry
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private protected ComplianceEvent() { }

    private protected ComplianceEvent(int? id, ApplicationUser? user) : base(id, user) { }

    public ICollection<EnforcementCase> EnforcementCases { get; } = [];

    // Data exchange properties
    public bool IsDataFlowEnabled => IsClosed && !IsDeleted && WorkEntryType != WorkEntryType.RmpInspection;

    [JsonIgnore]
    [StringLength(11)]
    public DataExchangeStatus DataExchangeStatus { get; init; }
}
