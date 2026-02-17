using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Compliance.Fces.Search;

public interface IFceSearchService : ISearchService<FceSearchDto, FceSearchResultDto, FceExportDto>;

public sealed class FceSearchService(
    IFceRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : BaseSearchService<Fce, FceSearchDto, FceSearchResultDto, FceExportDto, IFceRepository>
        (repository, facilityService, mapper, userService, authorization),
        IFceSearchService
{
    private readonly IFceRepository _repository = repository;

    public Task<IPaginatedResult<FceSearchResultDto>> SearchAsync(FceSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default) =>
        SearchAsync(spec, paging, loadFacilities, FceFilters.SearchPredicate, CompliancePolicies.ComplianceManager,
            token);

    public Task<int> CountAsync(FceSearchDto spec, CancellationToken token = default) =>
        CountAsync(spec, FceFilters.SearchPredicate, CompliancePolicies.ComplianceManager, token);

    public Task<IEnumerable<FceExportDto>> ExportAsync(FceSearchDto spec, CancellationToken token = default) =>
        ExportAsync(spec, FceFilters.SearchPredicate, CompliancePolicies.ComplianceManager, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => _repository.Dispose();
    public async ValueTask DisposeAsync() => await _repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
