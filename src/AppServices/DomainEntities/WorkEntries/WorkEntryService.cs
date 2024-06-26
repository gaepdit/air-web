using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.DomainEntities.WorkEntries.BaseWorkEntryDto;
using AirWeb.AppServices.DomainEntities.WorkEntries.Inspections;
using AirWeb.AppServices.DomainEntities.WorkEntries.Notifications;
using AirWeb.AppServices.DomainEntities.WorkEntries.PermitRevocations;
using AirWeb.AppServices.DomainEntities.WorkEntries.Reports;
using AirWeb.AppServices.DomainEntities.WorkEntries.RmpInspections;
using AirWeb.AppServices.DomainEntities.WorkEntries.Search;
using AirWeb.AppServices.DomainEntities.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.ValueObjects;
using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace AirWeb.AppServices.DomainEntities.WorkEntries;

public sealed partial class WorkEntryService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IWorkEntryRepository workEntryRepository,
    IWorkEntryManager workEntryManager,
    INotificationService notificationService,
    IFacilityRepository facilityRepository,
    IUserService userService,
    IAuthorizationService authorization) : IWorkEntryService
{
    // Query
    public async Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        if (!await workEntryRepository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        return await workEntryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false) switch
        {
            WorkEntryType.Notification => mapper.Map<NotificationViewDto>(await workEntryRepository
                .FindAsync<Notification>(id, token).ConfigureAwait(false)),
            WorkEntryType.PermitRevocation => mapper.Map<PermitRevocationViewDto>(await workEntryRepository
                .FindAsync<PermitRevocation>(id, token).ConfigureAwait(false)),
            WorkEntryType.ComplianceEvent => await FindComplianceEventAsync(id, token).ConfigureAwait(false),

            _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
        };
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
        Expression<Func<BaseWorkEntry, bool>> predicate, CancellationToken token)
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
        var workEntry = await CreateWorkEntryFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);
        await workEntryRepository.InsertAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return new CreateResultDto<int>(workEntry.Id)
        {
            NotificationResult = await NotifyOwnerAsync(workEntry, Template.NewEntry, token).ConfigureAwait(false),
        };
    }

    public async Task<NotificationResult> UpdateAsync(int id, IWorkEntryUpdateDto resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(id, token).ConfigureAwait(false);
        workEntry.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await UpdateWorkEntryFromDtoAsync(resource, workEntry, token).ConfigureAwait(false);
        await workEntryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        return await NotifyOwnerAsync(workEntry, Template.UpdatedEntry, token).ConfigureAwait(false);
    }

    public async Task<NotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var comment = workEntryManager.CreateComment(resource.Comment, currentUser);
        await workEntryRepository.AddCommentAsync(id, comment, token).ConfigureAwait(false);

        var workEntry = await workEntryRepository.GetAsync(id, token).ConfigureAwait(false);
        return await NotifyOwnerAsync(workEntry, Template.CommentAdded, token, comment).ConfigureAwait(false);
    }

    public async Task<NotificationResult> CloseAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var workEntry = await workEntryRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        workEntryManager.Close(workEntry, currentUser);
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
        workEntryManager.Restore(workEntry);
        await workEntryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return NotificationResult.UndefinedResult();
    }

    private async Task<NotificationResult> NotifyOwnerAsync(BaseWorkEntry baseWorkEntry, Template template,
        CancellationToken token, Comment? comment = null)
    {
        var recipient = baseWorkEntry.ResponsibleStaff;

        if (recipient is null)
            return NotificationResult.FailureResult("This Work Entry does not have an available recipient.");
        if (!recipient.Active)
            return NotificationResult.FailureResult("The Work Entry recipient is not an active user.");
        if (recipient.Email is null)
            return NotificationResult.FailureResult("The Work Entry recipient cannot be emailed.");

        return comment is null
            ? await notificationService.SendNotificationAsync(template, recipient.Email, token,
                baseWorkEntry.Id.ToString()).ConfigureAwait(false)
            : await notificationService.SendNotificationAsync(template, recipient.Email, token,
                    baseWorkEntry.Id.ToString(), comment.Text, comment.CommentedAt.ToString(),
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
