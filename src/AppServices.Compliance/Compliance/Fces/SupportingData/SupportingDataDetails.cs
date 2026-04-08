using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using IaipDataService.PermitFees;

namespace AirWeb.AppServices.Compliance.Compliance.Fces.SupportingData;

public record SupportingDataDetails
{
    public required IReadOnlyCollection<ComplianceWorkSearchResultDto> ComplianceSummary { get; init; }
    public required IReadOnlyCollection<CaseFileSearchResultDto> CaseFileSummary { get; init; }
    public required IReadOnlyCollection<AnnualFeeSummary> AnnualFeesSummary { get; init; }
}
