using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Search;

public sealed partial class ComplianceSearchService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IWorkEntryRepository workEntryRepository,
    IWorkEntryManager workEntryManager,
    INotificationService notificationService,
    IFacilityRepository facilityRepository,
    IUserService userService,
    IAuthorizationService authorization) : IComplianceSearchService
{
    public async Task<IPaginatedResult<ComplianceSearchResultDto>> SearchAsync(ComplianceSearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;
        return await PerformPagedSearchAsync(paging, ComplianceSearchFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    private async Task<IPaginatedResult<ComplianceSearchResultDto>> PerformPagedSearchAsync(PaginatedRequest paging,
        Expression<Func<WorkEntry, bool>> predicate, CancellationToken token)
    {
        var count = await workEntryRepository.CountAsync(predicate, token).ConfigureAwait(false);
        var items = count > 0
            ? mapper.Map<IEnumerable<ComplianceSearchResultDto>>(await workEntryRepository
                .GetPagedListAsync(predicate, paging, token).ConfigureAwait(false))
            : [];
        return new PaginatedResult<ComplianceSearchResultDto>(items, count, paging);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        workEntryRepository.Dispose();
        facilityRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await workEntryRepository.DisposeAsync().ConfigureAwait(false);
        await facilityRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
