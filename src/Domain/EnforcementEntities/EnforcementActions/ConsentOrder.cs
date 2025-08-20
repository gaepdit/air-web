using AirWeb.Domain.DataAttributes;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions.ActionProperties;
using AirWeb.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AirWeb.Domain.EnforcementEntities.EnforcementActions;

public class ConsentOrder : EnforcementAction, IFormalEnforcementAction
{
    // Constructors
    [UsedImplicitly] // Used by ORM.
    private ConsentOrder() { }

    internal ConsentOrder(Guid id, CaseFile caseFile, ApplicationUser? user)
        : base(id, caseFile, user)
    {
        ActionType = EnforcementActionType.ConsentOrder;
    }

    public DateOnly? ReceivedFromFacility { get; set; }
    public DateOnly? ExecutedDate { get; set; }
    public bool IsExecuted => ExecutedDate.HasValue;
    public void Execute(DateOnly executedDate) => ExecutedDate = executedDate;

    public DateOnly? ReceivedFromDirectorsOffice { get; set; }
    public DateOnly? ResolvedDate { get; set; }
    public bool IsResolved => ResolvedDate.HasValue;
    public void Resolve(DateOnly resolvedDate) => ResolvedDate = resolvedDate;

    [PositiveShort(ErrorMessage = "The Order ID must be a positive number.")]
    public short OrderId { get; set; }

    public const string? OrderNumberPrefix = "EPD-AQC-";

    [StringLength(13)]
    public string? OrderNumber
    {
        get => OrderId == 0 ? null : $"{OrderNumberPrefix}{OrderId}";

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

    [Precision(precision: 12, scale: 2)]
    [PositiveDecimal(ErrorMessage = "The penalty amount cannot be negative.")]
    public decimal? PenaltyAmount { get; init; }

    [StringLength(7000)]
    public string? PenaltyComment { get; init; }

    public bool StipulatedPenaltiesDefined { get; init; }
    public List<StipulatedPenalty> StipulatedPenalties { get; } = [];
}
