using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.CommonSearch;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.WorkEntries.Search;

public interface IWorkEntrySearchService
    : ISearchService<WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto>;

public sealed class WorkEntrySearchService(
    IComplianceWorkRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : BaseSearchService<ComplianceWork, WorkEntrySearchDto, WorkEntrySearchResultDto, WorkEntryExportDto,
            IComplianceWorkRepository>
        (repository, facilityService, mapper, userService, authorization),
        IWorkEntrySearchService
{
    private readonly IComplianceWorkRepository _repository = repository;

    public Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default) =>
        SearchAsync(spec, paging, loadFacilities, WorkEntryFilters.SearchPredicate, Policies.ComplianceManager, token);

    public Task<int> CountAsync(WorkEntrySearchDto spec, CancellationToken token = default) =>
        CountAsync(spec, WorkEntryFilters.SearchPredicate, Policies.ComplianceManager, token);

    public Task<IEnumerable<WorkEntryExportDto>> ExportAsync(WorkEntrySearchDto spec,
        CancellationToken token = default) =>
        ExportAsync(spec, WorkEntryFilters.SearchPredicate, Policies.ComplianceManager, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => _repository.Dispose();
    public async ValueTask DisposeAsync() => await _repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
