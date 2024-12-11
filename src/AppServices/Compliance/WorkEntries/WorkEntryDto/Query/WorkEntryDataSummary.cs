using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query.SupportingData;
using AirWeb.AppServices.Enforcement;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;

public record WorkEntryDataSummary
{
    // Compliance data
    public IEnumerable<AccViewDto> Accs { get; init; } = [];
    public IEnumerable<InspectionViewDto> Inspections { get; init; } = [];
    public IEnumerable<NotificationViewDto> Notifications { get; init; } = [];
    public IEnumerable<ReportViewDto> Reports { get; init; } = [];
    public IEnumerable<InspectionViewDto> RmpInspections { get; init; } = [];

    // To be implemented
    public IEnumerable<EnforcementSummaryDto> EnforcementHistory { get; init; } = [];

    // IAIP data
    public IEnumerable<FeeYearSummaryDto> FeesHistory { get; init; } = [];

    // Combined compliance and IAIP data
    public IEnumerable<SourceTestSummaryDto> SourceTests { get; init; } = [];
}
