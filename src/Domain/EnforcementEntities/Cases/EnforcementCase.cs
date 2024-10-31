using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.Properties;
using AirWeb.Domain.Identity;
using System.ComponentModel;

namespace AirWeb.Domain.EnforcementEntities.Cases;

public class EnforcementCase : AuditableSoftDeleteEntity<int>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private EnforcementCase() { }

    internal EnforcementCase(int? id, FacilityId facilityId)
    {
        if (id is not null) Id = id.Value;
        FacilityId = facilityId;
    }

    // Facility properties
    [StringLength(9)]
    public string FacilityId { get; private set; } = string.Empty;

    private Facility _facility = default!;

    [NotMapped]
    public Facility Facility
    {
        get => _facility;
        set
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
    public DateOnly DiscoveryDate { get; set; }
    public DateOnly DayZero { get; set; }

    // Programs & pollutants
    public ICollection<Pollutant> GetPollutants() => Properties.Data.Pollutants
        .Where(pollutant => PollutantIds.Contains(pollutant.Code)).ToList();

    public ICollection<string> PollutantIds { get; init; } = [];

    public ICollection<AirProgram> AirPrograms { get; init; } = [];

    // Comments
    public ICollection<EnforcementCaseComment> EnforcementComments { get; set; } = [];

    // Compliance Event & Enforcement Action relationships
    public ICollection<ComplianceEvent> ComplianceEvents { get; set; } = [];
    public ICollection<ComplianceEventEnforcementLinkage> ComplianceEventEnforcementLinkages { get; set; } = [];
    public ICollection<EnforcementAction> EnforcementActions { get; set; } = [];

    // Closure properties
    public bool IsClosed { get; internal set; }
    public ApplicationUser? ClosedBy { get; internal set; }
    public DateOnly? ClosedDate { get; internal set; }

    internal void Close(ApplicationUser? user)
    {
        IsClosed = true;
        ClosedDate = DateOnly.FromDateTime(DateTime.Now);
        ClosedBy = user;
    }

    internal void Reopen()
    {
        IsClosed = false;
        ClosedDate = null;
        ClosedBy = null;
    }

    // Deletion properties
    public ApplicationUser? DeletedBy { get; set; }

    [StringLength(7000)]
    public string? DeleteComments { get; set; }

    // Data flow properties
    public short? AfsKeyActionNumber { get; set; }
    public string CaseFileId => Facility.GetEpaIdFromActionNumber(AfsKeyActionNumber);
}

public enum EnforcementCaseStatus
{
    [Description("Open enforcement case")] CaseOpen,
    [Description("Subject to compliance schedule")] SubjectToComplianceSchedule,
    [Description("Enforcement case resolved")] CaseResolved,
    [Description("Enforcement case closed")] CaseClosed,
}
