using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Search;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.RmpInspections;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public sealed partial class WorkEntryService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IWorkEntryRepository workEntryRepository,
    IWorkEntryManager workEntryManager,
    IAppNotificationService appNotificationService,
    IFacilityRepository facilityRepository,
    IUserService userService,
    IAuthorizationService authorization) : IWorkEntryService
{
    // Query
    public async Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        if (!await workEntryRepository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        var entry = await workEntryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false) switch
        {
            WorkEntryType.Notification => mapper.Map<NotificationViewDto>(await workEntryRepository
                .FindAsync<Notification>(id, token).ConfigureAwait(false)),
            WorkEntryType.PermitRevocation => mapper.Map<PermitRevocationViewDto>(await workEntryRepository
                .FindAsync<PermitRevocation>(id, token).ConfigureAwait(false)),
            WorkEntryType.ComplianceEvent => await FindComplianceEventAsync(id, token).ConfigureAwait(false),

            _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
        };

        var facility = await facilityRepository.GetFacilityAsync((FacilityId)entry!.FacilityId, token)
            .ConfigureAwait(false);
        entry.Facility = mapper.Map<FacilityViewDto>(facility);
        return entry;
    }

    private async Task<IWorkEntryViewDto?> FindComplianceEventAsync(int id, CancellationToken token) =>
        await workEntryRepository.GetComplianceEventTypeAsync(id, token).ConfigureAwait(false) switch
        {
            ComplianceEventType.Inspection => mapper.Map<InspectionViewDto>(await workEntryRepository
                .FindAsync<Inspection>(id, token).ConfigureAwait(false)),
            ComplianceEventType.Report => mapper.Map<ReportViewDto>(await workEntryRepository
                .FindAsync<Report>(id, token).ConfigureAwait(false)),
            ComplianceEventType.SourceTestReview => mapper.Map<SourceTestReviewViewDto>(await workEntryRepository
                .FindAsync<SourceTestReview>(id, token).ConfigureAwait(false)),
            ComplianceEventType.AnnualComplianceCertification => mapper.Map<InspectionViewDto>(await workEntryRepository
                .FindAsync<AnnualComplianceCertification>(id, token).ConfigureAwait(false)),
            ComplianceEventType.RmpInspection => mapper.Map<RmpInspectionViewDto>(await workEntryRepository
                .FindAsync<RmpInspection>(id, token).ConfigureAwait(false)),

            _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Compliance Event Type."),
        };

    public async Task<IWorkEntryUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default)
    {
        if (!await workEntryRepository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        return await workEntryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false) switch
        {
            WorkEntryType.Notification => mapper.Map<NotificationUpdateDto>(await workEntryRepository
                .FindAsync<Notification>(id, token).ConfigureAwait(false)),
            WorkEntryType.PermitRevocation => mapper.Map<PermitRevocationUpdateDto>(await workEntryRepository
                .FindAsync<PermitRevocation>(id, token).ConfigureAwait(false)),
            WorkEntryType.ComplianceEvent => await FindComplianceEventForUpdateAsync(id, token).ConfigureAwait(false),

            _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
        };
    }

    private async Task<IWorkEntryUpdateDto?> FindComplianceEventForUpdateAsync(int id, CancellationToken token) =>
        await workEntryRepository.GetComplianceEventTypeAsync(id, token).ConfigureAwait(false) switch
        {
            ComplianceEventType.Inspection => mapper.Map<InspectionUpdateDto>(await workEntryRepository
                .FindAsync<Inspection>(id, token).ConfigureAwait(false)),
            ComplianceEventType.Report => mapper.Map<ReportUpdateDto>(await workEntryRepository
                .FindAsync<Report>(id, token).ConfigureAwait(false)),
            ComplianceEventType.SourceTestReview => mapper.Map<SourceTestReviewUpdateDto>(await workEntryRepository
                .FindAsync<SourceTestReview>(id, token).ConfigureAwait(false)),
            ComplianceEventType.AnnualComplianceCertification => mapper.Map<InspectionUpdateDto>(
                await workEntryRepository
                    .FindAsync<AnnualComplianceCertification>(id, token).ConfigureAwait(false)),
            ComplianceEventType.RmpInspection => mapper.Map<RmpInspectionUpdateDto>(await workEntryRepository
                .FindAsync<RmpInspection>(id, token).ConfigureAwait(false)),

            _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Compliance Event Type."),
        };

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

    // Command
    public async Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = await CreateWorkEntryFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);
        await workEntryRepository.InsertAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return new CreateResult<int>(workEntry.Id,
            await NotifyOwnerAsync(workEntry, Template.EntryCreated, token).ConfigureAwait(false));
    }

    public async Task<AppNotificationResult> UpdateAsync(int id, IWorkEntryUpdateDto resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(id, token).ConfigureAwait(false);
        workEntry.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await UpdateWorkEntryFromDtoAsync(resource, workEntry, token).ConfigureAwait(false);
        await workEntryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        return await NotifyOwnerAsync(workEntry, Template.EntryUpdated, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var comment = Comment.CreateComment(resource.Comment, currentUser);
        await workEntryRepository.AddCommentAsync(id, comment, token).ConfigureAwait(false);

        var workEntry = await workEntryRepository.GetAsync(id, token).ConfigureAwait(false);
        return await NotifyOwnerAsync(workEntry, Template.EntryCommentAdded, token, comment).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> CloseAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Close(workEntry, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(workEntry, Template.EntryClosed, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> ReopenAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Reopen(workEntry, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(workEntry, Template.EntryReopened, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Delete(workEntry, resource.Comment, currentUser);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(workEntry, Template.EntryDeleted, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        workEntryManager.Restore(workEntry);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(workEntry, Template.EntryRestored, token).ConfigureAwait(false);
    }

    private async Task<AppNotificationResult> NotifyOwnerAsync(WorkEntry workEntry, Template template,
        CancellationToken token, Comment? comment = null)
    {
        var recipient = workEntry.ResponsibleStaff;

        if (recipient is null)
            return AppNotificationResult.FailureResult("This Work Entry does not have an available recipient.");
        if (!recipient.Active)
            return AppNotificationResult.FailureResult("The Work Entry recipient is not an active user.");
        if (recipient.Email is null)
            return AppNotificationResult.FailureResult("The Work Entry recipient cannot be emailed.");

        return comment is null
            ? await appNotificationService.SendNotificationAsync(template, recipient.Email, token,
                workEntry.Id.ToString()).ConfigureAwait(false)
            : await appNotificationService.SendNotificationAsync(template, recipient.Email, token,
                    workEntry.Id.ToString(), comment.Text, comment.CommentedAt.ToString(),
                    comment.CommentBy?.FullName)
                .ConfigureAwait(false);
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
