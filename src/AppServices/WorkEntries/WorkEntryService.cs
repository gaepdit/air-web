using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.AppServices.WorkEntries.BaseCommandDto;
using AirWeb.AppServices.WorkEntries.BaseViewDto;
using AirWeb.AppServices.WorkEntries.SearchDto;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.Identity;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.WorkEntries;

public sealed class WorkEntryService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IWorkEntryRepository workEntryRepository,
    IWorkEntryManager workEntryManager,
    INotificationService notificationService,
    IUserService userService,
    IAuthorizationService authorization) : IWorkEntryService
{
    // Query
    public async Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.FindIncludeAllAsync(id, token)
            .ConfigureAwait(false);
        return workEntry is null ? null : mapper.Map<IWorkEntryViewDto>(workEntry);
    }

    public async Task<IWorkEntryUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<IWorkEntryUpdateDto>(await workEntryRepository
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

    // Command
    public async Task<CreateResultDto<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = CreateWorkEntryFromDto(resource, currentUser);

        await workEntryRepository.InsertAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        var result = new CreateResultDto<int>(workEntry.Id);

        // Send notification
        var template = Template.NewEntry;
        result.NotificationResult = await NotifyOwnerAsync(workEntry, template, token).ConfigureAwait(false);

        return result;
    }

    public async Task<NotificationResult> UpdateAsync(int id, IWorkEntryUpdateDto resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(id, token).ConfigureAwait(false);
        workEntry.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        workEntry.Notes = resource.Notes;
        await workEntryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        return NotificationResult.UndefinedResult();
    }

    public Task<NotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<NotificationResult> CloseAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Close(workEntry, resource.Comment, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return NotificationResult.UndefinedResult();
    }

    public async Task<NotificationResult> ReopenAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Reopen(workEntry, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        // Send notification
        return await NotifyOwnerAsync(workEntry, Template.Reopened, token).ConfigureAwait(false);
    }

    public async Task<NotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Delete(workEntry, resource.Comment, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return NotificationResult.UndefinedResult();
    }

    public async Task<NotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Restore(workEntry, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return NotificationResult.UndefinedResult();
    }

    private WorkEntry CreateWorkEntryFromDto(IWorkEntryCreateDto resource, ApplicationUser? currentUser)
    {
        var workEntry = workEntryManager.Create(TODO, currentUser);
        workEntry.Notes = resource.Notes;
        return workEntry;
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
    }

    public async ValueTask DisposeAsync()
    {
        await workEntryRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
