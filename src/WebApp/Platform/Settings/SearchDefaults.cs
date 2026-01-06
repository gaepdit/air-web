using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Enforcement.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Platform.Settings;

internal static class SearchDefaults
{
    // Default pagination size for search results, etc.
    public const int PageSize = 25;

    // Default pagination size for summary tables.
    public const int SummaryTableSize = 15;

    public static WorkEntrySearchDto FacilityCompliance(string facilityId) =>
        new() { FacilityId = facilityId };

    public static FceSearchDto FacilityFces(string facilityId) =>
        new() { FacilityId = facilityId };

    public static CaseFileSearchDto FacilityEnforcement(string facilityId) =>
        new() { FacilityId = facilityId };

    public static WorkEntrySearchDto StaffOpenCompliance(string userId) =>
        new() { Staff = userId, Closed = ClosedOpenAny.Open };

    public static CaseFileSearchDto StaffOpenEnforcement(string userId) =>
        new() { Staff = userId, Closed = ClosedOpenAny.Open };

    public static WorkEntrySearchDto OfficeOpenCompliance(Guid officeId) =>
        new() { Office = officeId, Closed = ClosedOpenAny.Open };

    public static CaseFileSearchDto OfficeOpenEnforcement(Guid officeId) =>
        new() { Office = officeId, Closed = ClosedOpenAny.Open };
}

internal static class PaginationDefaults
{
    public static PaginatedRequest ComplianceSummary { get; } =
        new(pageNumber: 1, pageSize: SearchDefaults.SummaryTableSize,
            sorting: WorkEntrySortBy.EventDateDesc.GetDescription());

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
}
