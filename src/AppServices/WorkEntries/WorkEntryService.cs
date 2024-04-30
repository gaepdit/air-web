using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using MyApp.AppServices.Notifications;
using MyApp.AppServices.Permissions;
using MyApp.AppServices.Permissions.Helpers;
using MyApp.AppServices.UserServices;
using MyApp.AppServices.WorkEntries.CommandDto;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.Domain.Identity;
using System.Linq.Expressions;

namespace MyApp.AppServices.WorkEntries;

public sealed class WorkEntryService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IWorkEntryRepository workEntryRepository,
    IEntryTypeRepository entryTypeRepository,
    IWorkEntryManager workEntryManager,
    INotificationService notificationService,
    IUserService userService,
    IAuthorizationService authorization) : IWorkEntryService
{
    public async Task<WorkEntryViewDto?> FindAsync(Guid id, bool includeDeletedActions = false,
        CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            includeDeletedActions = false;
        var workEntry = await workEntryRepository.FindIncludeAllAsync(id, includeDeletedActions, token)
            .ConfigureAwait(false);
        return workEntry is null ? null : mapper.Map<WorkEntryViewDto>(workEntry);
    }

    public async Task<WorkEntryUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<WorkEntryUpdateDto>(await workEntryRepository
            .FindAsync(entry => entry.Id == id && !entry.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task<IPaginatedResult<WorkEntrySearchResultDto>> SearchAsync(WorkEntrySearchDto spec,
        PaginatedRequest paging, CancellationToken token = default)
    {
        var principal = userService.GetCurrentPrincipal();
        if (!await authorization.Succeeded(principal!, Policies.Manager).ConfigureAwait(false))
            spec.DeletedStatus = null;
        return await PerformPagedSearchAsync(paging, WorkEntryFilters.SearchPredicate(spec), token)
            .ConfigureAwait(false);
    }

    private async Task<IPaginatedResult<WorkEntrySearchResultDto>> PerformPagedSearchAsync(PaginatedRequest paging,
        Expression<Func<WorkEntry, bool>> predicate, CancellationToken token)
    {
        var count = await workEntryRepository.CountAsync(predicate, token).ConfigureAwait(false);
        var items = count > 0
            ? mapper.Map<IEnumerable<WorkEntrySearchResultDto>>(await workEntryRepository
                .GetPagedListAsync(predicate, paging, token).ConfigureAwait(false))
            : [];
        return new PaginatedResult<WorkEntrySearchResultDto>(items, count, paging);
    }

    public async Task<WorkEntryCreateResult> CreateAsync(WorkEntryCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = await CreateWorkEntryFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);

        await workEntryRepository.InsertAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        var result = new WorkEntryCreateResult(workEntry.Id);

        // Send notification
        var template = Template.NewEntry;
        var notificationResult = await NotifyOwnerAsync(workEntry, template, token).ConfigureAwait(false);
        if (!notificationResult.Success) result.AddWarning(notificationResult.FailureMessage);

        return result;
    }

    public async Task UpdateAsync(Guid id, WorkEntryUpdateDto resource, CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(id, token).ConfigureAwait(false);
        workEntry.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await MapWorkEntryDetailsAsync(workEntry, resource, token).ConfigureAwait(false);
        await workEntryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);
    }

    public async Task CloseAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.WorkEntryId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Close(workEntry, resource.Comments, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
    }

    public async Task<NotificationResult> ReopenAsync(WorkEntryChangeStatusDto resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.WorkEntryId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Reopen(workEntry, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        // Send notification
        return await NotifyOwnerAsync(workEntry, Template.Reopened, token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.WorkEntryId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Delete(workEntry, resource.Comments, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
    }

    public async Task RestoreAsync(WorkEntryChangeStatusDto resource, CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.WorkEntryId, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Restore(workEntry, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
    }

    private async Task<WorkEntry> CreateWorkEntryFromDtoAsync(WorkEntryCreateDto resource, ApplicationUser? currentUser,
        CancellationToken token)
    {
        var workEntry = workEntryManager.Create(currentUser);
        await MapWorkEntryDetailsAsync(workEntry, resource, token).ConfigureAwait(false);
        return workEntry;
    }

    private async Task MapWorkEntryDetailsAsync(WorkEntry workEntry, IWorkEntryCommandDto resource,
        CancellationToken token)
    {
        workEntry.EntryType = await entryTypeRepository.GetAsync(resource.EntryTypeId, token).ConfigureAwait(false);
        workEntry.Notes = resource.Notes;
    }

    private async Task<NotificationResult> NotifyOwnerAsync(WorkEntry workEntry, Template template,
        CancellationToken token)
    {
        var recipient = workEntry.ReceivedBy;

        if (recipient is null)
            return NotificationResult.FailureResult("This Work Entry does not have an available recipient.");
        if (!recipient.Active)
            return NotificationResult.FailureResult("The Work Entry recipient is not an active user.");
        if (recipient.Email is null)
            return NotificationResult.FailureResult("The Work Entry recipient cannot be emailed.");

        return await notificationService.SendNotificationAsync(template, recipient.Email, workEntry, token)
            .ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        workEntryRepository.Dispose();
        entryTypeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await workEntryRepository.DisposeAsync().ConfigureAwait(false);
        await entryTypeRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
