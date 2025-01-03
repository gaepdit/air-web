using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Data;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.Actions;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.Cases;

public class CaseFile : ClosableEntity<int>
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private CaseFile() { }

    internal CaseFile(int? id, FacilityId facilityId, ApplicationUser? user)
    {
        if (id is not null) Id = id.Value;
        FacilityId = facilityId;
        SetCreator(user?.Id);
    }

    // Basic data

    [StringLength(9)]
    public string FacilityId { get; } = null!;

    // Required for new cases but nullable for historical data.
    public ApplicationUser? ResponsibleStaff { get; set; }

    [StringLength(7000)]
    public string? Notes { get; set; }

    // Required if the data flow is enabled.
    [BackingField(nameof(_violationTypeCode))]
    public ViolationType? ViolationType
    {
        get => ViolationTypeData.GetViolationType(_violationTypeCode);
        set => _violationTypeCode = value?.Code;
    }

    [StringLength(5)]
    private string? _violationTypeCode;

    // Status

    [StringLength(27)]
    public CaseFileStatus CaseFileStatus
    {
        get
        {
            // TODO: Review the logic for this method.
            if (IsClosed) return CaseFileStatus.CaseClosed;

            if (EnforcementActions.Exists(action =>
                    action.ActionType is EnforcementActionType.ConsentOrder
                        or EnforcementActionType.AdministrativeOrder && ((IExecutable)action).IsExecuted))
            {
                return EnforcementActions.Exists(action => action.ActionType is EnforcementActionType.ConsentOrder
                    or EnforcementActionType.AdministrativeOrder && ((IExecutable)action).IsResolved)
                    ? CaseFileStatus.CaseResolved
                    : CaseFileStatus.SubjectToComplianceSchedule;
            }

            return CaseFileStatus.CaseOpen;
        }
        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
        [SuppressMessage("Blocker Code Smell", "S3237:\"value\" contextual keyword should be used")]
        private set
        {
            // Method intentionally left empty.
            // This allows storing read-only properties in the database.
            // See: https://github.com/dotnet/efcore/issues/13316#issuecomment-421052406
        }
    }

    // Required for new cases but nullable for historical data.
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
        [UsedImplicitly]
        [SuppressMessage("ReSharper", "ValueParameterNotUsed")]
        [SuppressMessage("Blocker Code Smell", "S3237:\"value\" contextual keyword should be used")]
        private set
        {
            // Method intentionally left empty.
            // This allows storing read-only properties in the database.
            // See: https://github.com/dotnet/efcore/issues/13316#issuecomment-421052406
        }
    }

    // Programs & pollutants
    public ICollection<Pollutant> GetPollutants() => CommonData.AllPollutants
        .Where(pollutant => PollutantIds.Contains(pollutant.Code)).ToList();

    public List<string> PollutantIds { get; } = [];

    public List<AirProgram> AirPrograms { get; } = [];

    // Comments
    public List<CaseFileComment> Comments { get; } = [];

    // Compliance Event & Enforcement Action relationships
    public ICollection<ComplianceEvent> ComplianceEvents { get; } = [];
    public List<EnforcementAction> EnforcementActions { get; } = [];

    // Data flow properties

    // Data flow is not used for LONs, Cases with no linked compliance event,
    // or Cases where the only linked compliance event is an RMP inspection.
    public bool IsDataFlowEnabled =>
        ComplianceEvents.Any(complianceEvent => complianceEvent.IsDataFlowEnabled) &&
        EnforcementActions.Exists(action => action.IsDataFlowEnabled);

    // Required if the data flow is enabled.
    public short? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; init; }
}

public enum CaseFileStatus
{
    [Description("Open enforcement case")] CaseOpen,
    [Description("Subject to compliance schedule")] SubjectToComplianceSchedule,
    [Description("Enforcement case resolved")] CaseResolved,
    [Description("Enforcement case closed")] CaseClosed,
}
