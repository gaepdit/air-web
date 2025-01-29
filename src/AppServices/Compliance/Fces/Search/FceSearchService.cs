using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.Fces;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces.Search;

public sealed class FceSearchService(
    IFceRepository repository,
    IFacilityService facilityService,
    IMapper mapper,
    IUserService userService,
    IAuthorizationService authorization)
    : ComplianceSearchService<Fce>(repository, facilityService, mapper, userService, authorization),
        IFceSearchService
{
    public async Task<IPaginatedResult<FceSearchResultDto>> SearchAsync(FceSearchDto spec,
        PaginatedRequest paging, bool loadFacilities = true, CancellationToken token = default)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await GetSearchResultsAsync<FceSearchResultDto>(paging, expression, loadFacilities, token)
            .ConfigureAwait(false);
    }

    public async Task<int> CountAsync(FceSearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await repository.CountAsync(expression, token).ConfigureAwait(false);
    }

    public async Task<IEnumerable<FceExportDto>> ExportAsync(FceSearchDto spec, CancellationToken token)
    {
        await CheckDeleteStatusAuth(spec).ConfigureAwait(false);
        var expression = FceFilters.SearchPredicate(spec.TrimAll());
        return await GetExportResultsAsync<FceExportDto>(expression, spec.Sort.GetDescription(), token)
            .ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
