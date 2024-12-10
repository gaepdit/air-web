using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.Domain.ValueObjects;

namespace AirWeb.AppServices.Compliance.Fces.SupportingData;

public record FceSupportingDataDto
{
    // Compliance data
    public IEnumerable<InspectionViewDto> Inspections { get; init; } = [];
    public IEnumerable<InspectionViewDto> RmpInspections { get; init; } = [];
    public IEnumerable<AccViewDto> Accs { get; init; } = [];
    public IEnumerable<ReportViewDto> Reports { get; init; } = [];
    public IEnumerable<NotificationViewDto> Notifications { get; init; } = [];
    public IEnumerable<SourceTestSummaryDto> SourceTests { get; init; } = [];

    // To be implemented
    public IEnumerable<EnforcementSummaryDto> EnforcementHistory { get; init; } = [];

    // IAIP data
    public IEnumerable<FeeYearSummaryDto> FeesHistory { get; init; } = [];
}
