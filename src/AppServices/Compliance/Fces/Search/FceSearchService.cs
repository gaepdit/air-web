using AirWeb.AppServices.CommonSearch;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.Fces;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces.Search;

public interface IFceSearchService : ISearchService<FceSearchDto, FceSearchResultDto, FceExportDto>;

public sealed class FceSearchService(
    IFceRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : BaseSearchService<Fce, FceSearchDto, FceSearchResultDto, FceExportDto>
        (repository, facilityService, mapper, userService, authorization),
        IFceSearchService
{
    public Task<IPaginatedResult<FceSearchResultDto>> SearchAsync(FceSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default) =>
        SearchAsync(spec, paging, loadFacilities, FceFilters.SearchPredicate, Policies.ComplianceManager, token);

    public Task<int> CountAsync(FceSearchDto spec, CancellationToken token = default) =>
        CountAsync(spec, FceFilters.SearchPredicate, Policies.ComplianceManager, token);

    public Task<IEnumerable<FceExportDto>> ExportAsync(FceSearchDto spec, CancellationToken token = default) =>
        ExportAsync(spec, FceFilters.SearchPredicate, Policies.ComplianceManager, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
