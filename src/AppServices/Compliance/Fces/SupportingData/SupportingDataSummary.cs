using IaipDataService.PermitFees;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record SupportingDataSummary
{
    // Compliance data
    public IEnumerable<AccSummaryDto> Accs { get; init; } = [];
    public IEnumerable<InspectionSummaryDto> Inspections { get; init; } = [];
    public IEnumerable<NotificationSummaryDto> Notifications { get; init; } = [];
    public IEnumerable<ReportSummaryDto> Reports { get; init; } = [];
    public IEnumerable<InspectionSummaryDto> RmpInspections { get; init; } = [];
    public IEnumerable<EnforcementCaseSummaryDto> EnforcementCases { get; init; } = [];

    // To be implemented

    // IAIP data
    public IEnumerable<AnnualFeeSummary> Fees { get; init; } = [];

    // Combined compliance and IAIP data
    public IEnumerable<SourceTestSummaryDto> SourceTests { get; init; } = [];
}
