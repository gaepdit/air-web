﻿using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.Properties;
using AirWeb.Domain.Identity;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.Cases;

public class EnforcementCase : ClosableEntity<int>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementCase() { }

    internal EnforcementCase(int? id, FacilityId facilityId, ApplicationUser? user)
    {
        if (id is not null) Id = id.Value;
        FacilityId = facilityId;
        SetCreator(user?.Id);
    }

    // Facility properties
    [StringLength(9)]
    public string FacilityId { get; private init; } = string.Empty;

    private readonly Facility _facility = default!;

    [NotMapped]
    public Facility Facility
    {
        get => _facility;
        init
        {
            _facility = value;
            FacilityId = value.Id;
        }
    }

    // Basic data
    public ApplicationUser? ResponsibleStaff { get; set; }

    [StringLength(7000)]
    public string Notes { get; set; } = string.Empty;

    [StringLength(3)]
    public string? ViolationTypeId { get; set; }

    private ViolationType _violationType = default!;

    [NotMapped]
    public ViolationType ViolationType
    {
        get => _violationType;
        set
        {
            _violationType = value;
            ViolationTypeId = value.Code;
        }
    }

    // Status
    public EnforcementCaseStatus Status { get; set; }
    public DateOnly? DiscoveryDate { get; set; }
    private DateOnly? MaxDayZero => DiscoveryDate?.AddDays(90);

    public DateOnly? DayZero
    {
        get
        {
            if (!IsDataFlowEnabled) return null;
            var actionDates = EnforcementActions
                .Where(action => action.IsDataFlowEnabled)
                .Select(action => action.IssueDate)
                .Append(MaxDayZero);
            var dates = actionDates.Where(date => date.HasValue).ToArray();
            return dates.Length == 0 ? null : dates.Min();
        }
    }

    // Programs & pollutants
    public ICollection<Pollutant> GetPollutants() => Properties.Data.Pollutants
        .Where(pollutant => PollutantIds.Contains(pollutant.Code)).ToList();

    public ICollection<string> PollutantIds { get; } = [];

    public ICollection<AirProgram> AirPrograms { get; } = [];

    // Comments
    public ICollection<EnforcementCaseComment> EnforcementComments { get; } = [];

    // Compliance Event & Enforcement Action relationships
    public ICollection<ComplianceEvent> ComplianceEvents { get; } = [];
    public ICollection<EnforcementAction> EnforcementActions { get; } = [];

    // Data flow properties

    // Data flow is not used for LONs, Cases with no linked compliance event,
    // or Cases where the only linked compliance event is an RMP inspection.
    public bool IsDataFlowEnabled =>
        ComplianceEvents.Any(complianceEvent => complianceEvent.IsDataFlowEnabled) &&
        EnforcementActions.Any(action => action.IsDataFlowEnabled);

    public short? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(11)]
    public DataExchangeStatus DataExchangeStatus { get; init; }
}

public enum EnforcementCaseStatus
{
    [Description("Open enforcement case")] CaseOpen,
    [Description("Subject to compliance schedule")] SubjectToComplianceSchedule,
    [Description("Enforcement case resolved")] CaseResolved,
    [Description("Enforcement case closed")] CaseClosed,
}
