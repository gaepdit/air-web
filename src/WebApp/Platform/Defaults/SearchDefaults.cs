using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Compliance.Fces.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.AppServices.Enforcement.Search;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Platform.Defaults;

internal static class SearchDefaults
{
    public const int SummaryTableSize = 15;

    public static WorkEntrySearchDto FacilityCompliance(string facilityId) =>
        new() { PartialFacilityId = facilityId };

    public static FceSearchDto FacilityFces(string facilityId) =>
        new() { PartialFacilityId = facilityId };

    public static CaseFileSearchDto FacilityEnforcement(string facilityId) =>
        new() { PartialFacilityId = facilityId };

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
}
