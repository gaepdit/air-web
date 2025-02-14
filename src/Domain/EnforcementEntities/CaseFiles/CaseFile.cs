using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Data;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.EnforcementEntities.CaseFiles;

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

    // Required if the data exchange is enabled.
    [BackingField(nameof(_violationTypeCode))]
    public ViolationType? ViolationType
    {
        get => ViolationTypeData.GetViolationType(_violationTypeCode);
        internal set => _violationTypeCode = value?.Code;
    }

    [StringLength(5)]
    private string? _violationTypeCode;

    // Status

    [StringLength(27)]
    public CaseFileStatus CaseFileStatus
    {
        get
        {
            if (IsClosed) return CaseFileStatus.Closed;

            if (EnforcementActions.Exists(action =>
                    action.ActionType is EnforcementActionType.ConsentOrder
                        or EnforcementActionType.AdministrativeOrder && ((IFormalEnforcementAction)action).IsExecuted))
            {
                return CaseFileStatus.SubjectToComplianceSchedule;
            }

            if (EnforcementActions.Exists(action => action.IsIssued))
            {
                return CaseFileStatus.Open;
            }

            return CaseFileStatus.Draft;
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

    // Computed dates

    // Required if the data exchange is enabled.
    public DateOnly? DayZero
    {
        get
        {
            if (!IsReportable) return null;
            var actionDates = EnforcementActions
                .Where(action => action.IsReportable)
                .Select(action => action.IssueDate) // List the dates each formal enforcement action was issued.
                .Append(DiscoveryDate?.AddDays(90)); // Add the max Day Zero.
            var dates = actionDates.Where(date => date.HasValue).ToArray();
            return dates.Length == 0 ? null : dates.Min(); // Day Zero is the earliest of the above list of dates.
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

    public DateOnly? EnforcementDate
    {
        get
        {
            var actionDates = EnforcementActions
                .Where(action => !action.IsDeleted)
                .Where(action => action.IsIssued)
                .Select(action => action.IssueDate); // List the dates each enforcement action was issued.
            var dates = actionDates.Where(date => date.HasValue).ToArray();
            return
                dates.Length == 0 ? null : dates.Min(); // Enforcement Date is the earliest of the above list of dates.
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
    // Required if the data exchange is enabled.
    public ICollection<Pollutant> GetPollutants() => CommonData.AllPollutants
        .Where(pollutant => PollutantIds.Contains(pollutant.Code)).ToList();

    public List<string> PollutantIds { get; } = [];
    public List<AirProgram> AirPrograms { get; } = [];

    // Comments
    public List<CaseFileComment> Comments { get; } = [];

    // Compliance Event & Enforcement Action relationships
    public ICollection<ComplianceEvent> ComplianceEvents { get; } = [];
    public List<EnforcementAction> EnforcementActions { get; } = [];

    // Data exchange properties

    // Data exchange is not used for LONs, Cases with no linked compliance event,
    // or Cases where the only linked compliance event is an RMP inspection.
    public bool IsReportable =>
        ComplianceEvents.Any(complianceEvent => complianceEvent.IsReportable) &&
        EnforcementActions.Exists(action => action.IsReportable);

    // Required if the data exchange is enabled.
    public short? ActionNumber { get; set; }

    [JsonIgnore]
    [StringLength(1)]
    public DataExchangeStatus DataExchangeStatus { get; init; }
}

public enum CaseFileStatus
{
    [Description("Draft")] Draft,
    [Description("Open")] Open,
    [Description("Subject to compliance schedule")] SubjectToComplianceSchedule,
    [Description("Closed")] Closed,
}
