using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;

public interface IComplianceWorkSearchService
    : ISearchService<ComplianceWorkSearchDto, ComplianceWorkSearchResultDto, ComplianceWorkExportDto>;

public sealed class ComplianceWorkSearchService(
    IComplianceWorkRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : BaseSearchService<ComplianceWork, ComplianceWorkSearchDto, ComplianceWorkSearchResultDto, ComplianceWorkExportDto,
            IComplianceWorkRepository>
        (repository, facilityService, mapper, userService, authorization),
        IComplianceWorkSearchService
{
    private readonly IComplianceWorkRepository _repository = repository;

    public Task<IPaginatedResult<ComplianceWorkSearchResultDto>> SearchAsync(ComplianceWorkSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default) =>
        SearchAsync(spec, paging, loadFacilities, ComplianceWorkFilters.SearchPredicate,
            CompliancePolicies.ComplianceManager, token);

    public Task<int> CountAsync(ComplianceWorkSearchDto spec, CancellationToken token = default) =>
        CountAsync(spec, ComplianceWorkFilters.SearchPredicate, CompliancePolicies.ComplianceManager, token);

    public Task<IEnumerable<ComplianceWorkExportDto>> ExportAsync(ComplianceWorkSearchDto spec,
        CancellationToken token = default) =>
        ExportAsync(spec, ComplianceWorkFilters.SearchPredicate, CompliancePolicies.ComplianceManager, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => _repository.Dispose();
    public async ValueTask DisposeAsync() => await _repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
