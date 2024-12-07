using AirWeb.AppServices.Reports.ComplianceDto.WorkItems;
using AirWeb.Domain.ValueObjects;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Reports.ComplianceDto;

public record FceReport
{
    [Display(Name = "FCE tracking number")]
    public int Id { get; init; }

    public Facility? Facility { get; set; }

    [Display(Name = "FCE year")]
    public int FceYear { get; init; }

    [Display(Name = "Reviewed by")]
    public PersonName StaffReviewedBy { get; set; }

    [Display(Name = "Date completed")]
    public DateTime DateCompleted { get; init; }

    [Display(Name = "On-site inspection conducted")]
    public bool WithOnsiteInspection { get; init; }

    public string Comments { get; init; } = "";

    public DateRange SupportingDataDateRange { get; set; }

    // Supporting compliance data

    public List<InspectionPartial> Inspections { get; init; } = [];
    public List<InspectionPartial> RmpInspections { get; init; } = [];
    public List<AccPartial> Accs { get; init; } = [];
    public List<ReportPartial> Reports { get; init; } = [];
    public List<NotificationPartial> Notifications { get; init; } = [];
    public List<StackTestWorkPartial> StackTests { get; init; } = [];
    public List<FeeYearPartial> FeesHistory { get; init; } = [];
    public List<EnforcementPartial> EnforcementHistory { get; init; } = [];
}
