namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record SupportingDataSummary
{
    // Compliance data
    public IEnumerable<AccSummaryDto> Accs { get; init; } = [];
    public IEnumerable<InspectionSummaryDto> Inspections { get; init; } = [];
    public IEnumerable<NotificationSummaryDto> Notifications { get; init; } = [];
    public IEnumerable<ReportSummaryDto> Reports { get; init; } = [];
    public IEnumerable<InspectionSummaryDto> RmpInspections { get; init; } = [];

    // To be implemented
    public IEnumerable<EnforcementCaseSummaryDto> EnforcementCases { get; init; } = [];

    // IAIP data
    public IEnumerable<FeeYearSummaryDto> Fees { get; init; } = [];

    // Combined compliance and IAIP data
    public IEnumerable<SourceTestSummaryDto> SourceTests { get; init; } = [];
}
