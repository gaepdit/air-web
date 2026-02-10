using AirWeb.AppServices.AuditPoints;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.DataExchange;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces;

public record FceViewDto : IFceBasicViewDto, IDataExchangeAction
{
    [Display(Name = "FCE Tracking Number")]
    public int Id { get; init; }

    public string FacilityId { get; init; } = null!;
    public string? FacilityName { get; set; }

    [Display(Name = "FCE Year")]
    public int Year { get; init; }

    [Display(Name = "Reviewed By")]
    public StaffViewDto? ReviewedBy { get; init; }

    [Display(Name = "Date Completed")]
    public DateOnly CompletedDate { get; init; }

    [Display(Name = "With On-Site Inspection")]
    public bool OnsiteInspection { get; init; }

    public string Notes { get; init; } = null!;

    [UsedImplicitly]
    public List<CommentViewDto> Comments { get; } = [];

    public List<AuditPointViewDto> AuditPoints { get; } = [];

    // Properties: Deletion
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }

    // Calculated properties
    public string OwnerId => ReviewedBy?.Id ?? string.Empty;
    public DateOnly SupportingDataStartDate => CompletedDate.AddYears(-Fce.DataPeriod);
    public DateRange SupportingDataDateRange => new(SupportingDataStartDate, CompletedDate);
    public DateOnly ExtendedDataStartDate => CompletedDate.AddYears(-Fce.ExtendedDataPeriod);
    public DateRange ExtendedDataDateRange => new(ExtendedDataStartDate, CompletedDate);

    // Data exchange
    public ushort? ActionNumber { get; set; }
    public DataExchangeStatus DataExchangeStatus { get; set; }
    public DateTimeOffset? DataExchangeStatusDate { get; set; }
}
