using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Search;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Enforcement.Search;

public interface ICaseFileSearchService : ISearchService<CaseFileSearchDto, CaseFileSearchResultDto, CaseFileExportDto>;

public sealed class CaseFileSearchService(
    ICaseFileRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization) :
    BaseSearchService<CaseFile, CaseFileSearchDto, CaseFileSearchResultDto, CaseFileExportDto, ICaseFileRepository>
    (repository, facilityService, mapper, userService, authorization),
    ICaseFileSearchService
{
    private readonly ICaseFileRepository _repository = repository;

    public Task<IPaginatedResult<CaseFileSearchResultDto>> SearchAsync(CaseFileSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default) =>
        SearchAsync(spec, paging, loadFacilities, CaseFileFilters.SearchPredicate,
            CompliancePolicies.EnforcementManager, token);

    public Task<int> CountAsync(CaseFileSearchDto spec, CancellationToken token = default) =>
        CountAsync(spec, CaseFileFilters.SearchPredicate, CompliancePolicies.EnforcementManager, token);

    public Task<IEnumerable<CaseFileExportDto>> ExportAsync(CaseFileSearchDto spec, CancellationToken token = default)
        => ExportAsync(spec, CaseFileFilters.SearchPredicate, CompliancePolicies.EnforcementManager, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => _repository.Dispose();
    public async ValueTask DisposeAsync() => await _repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
