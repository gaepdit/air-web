using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record FceSupportingDataDto
{
    // The number of years covered by the FCE
    public const int FceDataPeriod = 1; // One year

    public IEnumerable<InspectionViewDto> Inspections { get; init; } = [];
    public IEnumerable<InspectionViewDto> RmpInspections { get; init; } = [];
    public IEnumerable<AccViewDto> Accs { get; init; } = [];
    public IEnumerable<ReportViewDto> Reports { get; init; } = [];
    public IEnumerable<NotificationViewDto> Notifications { get; init; } = [];
    public IEnumerable<SourceTestSummaryDto> SourceTests { get; init; } = [];

    // The number of years of additional data retrieved
    // (fees history and enforcement history)
    public const int FceExtendedDataPeriod = 5; // Five years

    public IEnumerable<FeeYearSummaryDto> FeesHistory { get; init; } = [];
    public IEnumerable<EnforcementSummaryDto> EnforcementHistory { get; init; } = [];
}
