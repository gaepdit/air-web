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
    public IEnumerable<AccViewDto> Accs { get; set; } = [];
    public IEnumerable<InspectionViewDto> Inspections { get; set; } = [];
    public IEnumerable<NotificationViewDto> Notifications { get; set; } = [];
    public IEnumerable<ReportViewDto> Reports { get; set; } = [];
    public IEnumerable<InspectionViewDto> RmpInspections { get; set; } = [];

    // To be implemented
    public IEnumerable<EnforcementSummaryDto> EnforcementHistory { get; set; } = [];

    // IAIP data
    public IEnumerable<FeeYearSummaryDto> FeesHistory { get; set; } = [];

    // Combined compliance and IAIP data
    public IEnumerable<SourceTestSummaryDto> SourceTests { get; set; } = [];
}
