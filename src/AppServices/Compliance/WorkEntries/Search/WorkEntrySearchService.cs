using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public interface IWorkEntrySearchService
    : ISearchService<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>;

public sealed class WorkEntrySearchService(
    IWorkEntryRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : BaseSearchService<WorkEntry, WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>
        (repository, facilityService, mapper, userService, authorization),
        IWorkEntrySearchService
{
    public Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default) =>
        SearchAsync(spec, paging, loadFacilities, WorkEntryFilters.SearchPredicate, Policies.ComplianceManager, token);

    public Task<int> CountAsync(WorkEntrySearchDto spec, CancellationToken token = default) =>
        CountAsync(spec, WorkEntryFilters.SearchPredicate, Policies.ComplianceManager, token);

    public Task<IEnumerable<WorkEntryExportDto>> ExportAsync(WorkEntrySearchDto spec,
        CancellationToken token = default) =>
        ExportAsync(spec, WorkEntryFilters.SearchPredicate, Policies.ComplianceManager, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
