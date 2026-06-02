using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.AppServices.Core.Search;
using AirWeb.AppServices.Sbeap.Cases.Dto;
using GaEpd.AppLibrary.Pagination;
using System.Text.Json;

namespace AirWeb.WebApp.Platform.Settings;

internal static class SearchDefaults
{
    // Default pagination size for search results, etc.
    public const int PageSize = 25;

    // Default pagination size for summary tables.
    public const int SummaryTableSize = 15;

    public static ComplianceWorkSearchDto FacilityCompliance(string facilityId) =>
        new() { FacilityId = facilityId };

    public static FceSearchDto FacilityFces(string facilityId) =>
        new() { FacilityId = facilityId };

    public static CaseFileSearchDto FacilityEnforcement(string facilityId) =>
        new() { FacilityId = facilityId };

    public static ComplianceWorkSearchDto StaffOpenCompliance(string userId) =>
        new() { Staff = userId, Closed = ClosedOpenAny.Open };

    public static CaseFileSearchDto StaffOpenEnforcement(string userId) =>
        new() { Staff = userId, Closed = ClosedOpenAny.Open };

    public static ComplianceWorkSearchDto OfficeOpenCompliance(Guid officeId) =>
        new() { Office = officeId, Closed = ClosedOpenAny.Open };

    public static CaseFileSearchDto OfficeOpenEnforcement(Guid officeId) =>
        new() { Office = officeId, Closed = ClosedOpenAny.Open };

    public static CaseworkSearchDto SbeapOpenCases() =>
        new() { Status = CaseStatus.Open };
}

internal static class PaginationDefaults
{
    public static PaginatedRequest ComplianceSummary { get; } =
        new(pageNumber: 1, pageSize: SearchDefaults.SummaryTableSize,
            sorting: ComplianceWorkSortBy.EventDateDesc.GetDescription());

    public static PaginatedRequest SourceTestSummary { get; } =
        new(pageNumber: 1, pageSize: SearchDefaults.SummaryTableSize, "default");

    public static PaginatedRequest FceSummary { get; } =
        new(pageNumber: 1, pageSize: SearchDefaults.SummaryTableSize,
            sorting: FceSortBy.YearDesc.GetDescription());

    public static PaginatedRequest EnforcementSummary { get; } =
        new(pageNumber: 1, pageSize: SearchDefaults.SummaryTableSize,
            sorting: CaseFileSortBy.DiscoveryDateDesc.GetDescription());

    public static PaginatedRequest EnforcementBulk { get; } =
        new(pageNumber: 1, pageSize: 1000, sorting: "ReviewRequestedDate");

    public static PaginatedRequest SbeapCasesSummary { get; } =
        new(pageNumber: 1, pageSize: SearchDefaults.SummaryTableSize,
            sorting: CaseworkSortBy.OpenedDate.GetDescription());
}

internal static class SerializationDefaults
{
    public static JsonSerializerOptions Options { get; } = AppSettings.Env == "Development"
        ? new JsonSerializerOptions { WriteIndented = true }
        : new JsonSerializerOptions();
}
