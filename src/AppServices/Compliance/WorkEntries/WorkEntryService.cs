using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.AppServices.Compliance.WorkEntries.Notifications;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AutoMapper;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.WorkEntries;

public sealed partial class WorkEntryService(
    IMapper mapper,
    IWorkEntryRepository entryRepository,
    IWorkEntryManager entryManager,
    IFacilityService facilityService,
    ICommentService<int> commentService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : IWorkEntryService
{
    // Query
    public async Task<IWorkEntryViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        if (!await entryRepository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        IWorkEntryViewDto entry =
            await entryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false) switch
            {
                WorkEntryType.AnnualComplianceCertification => mapper.Map<AccViewDto>(await entryRepository
                    .FindWithCommentsAsync<AnnualComplianceCertification>(id, token).ConfigureAwait(false)),
                WorkEntryType.Inspection => mapper.Map<InspectionViewDto>(await entryRepository
                    .FindWithCommentsAsync<Inspection>(id, token).ConfigureAwait(false)),
                WorkEntryType.Notification => mapper.Map<NotificationViewDto>(await entryRepository
                    .FindWithCommentsAsync<Notification>(id, token).ConfigureAwait(false)),
                WorkEntryType.PermitRevocation => mapper.Map<PermitRevocationViewDto>(await entryRepository
                    .FindWithCommentsAsync<PermitRevocation>(id, token).ConfigureAwait(false)),
                WorkEntryType.Report => mapper.Map<ReportViewDto>(await entryRepository
                    .FindWithCommentsAsync<Report>(id, token).ConfigureAwait(false)),
                WorkEntryType.RmpInspection => mapper.Map<InspectionViewDto>(await entryRepository
                    .FindWithCommentsAsync<RmpInspection>(id, token).ConfigureAwait(false)),
                WorkEntryType.SourceTestReview => mapper.Map<SourceTestReviewViewDto>(await entryRepository
                    .FindWithCommentsAsync<SourceTestReview>(id, token).ConfigureAwait(false)),

                _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
            };

        entry.FacilityName = await facilityService.GetNameAsync((FacilityId)entry.FacilityId).ConfigureAwait(false);
        return entry;
    }

    public async Task<IWorkEntryCommandDto?> FindForUpdateAsync(int id, CancellationToken token = default)
    {
        if (!await entryRepository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        return await entryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false) switch
        {
            WorkEntryType.AnnualComplianceCertification => mapper.Map<AccUpdateDto>(await entryRepository
                .FindAsync<AnnualComplianceCertification>(id, token).ConfigureAwait(false)),
            WorkEntryType.Inspection => mapper.Map<InspectionUpdateDto>(await entryRepository
                .FindAsync<Inspection>(id, token).ConfigureAwait(false)),
            WorkEntryType.Notification => mapper.Map<NotificationUpdateDto>(await entryRepository
                .FindAsync<Notification>(id, token).ConfigureAwait(false)),
            WorkEntryType.PermitRevocation => mapper.Map<PermitRevocationUpdateDto>(
                await entryRepository.FindAsync<PermitRevocation>(id, token).ConfigureAwait(false)),
            WorkEntryType.Report => mapper.Map<ReportUpdateDto>(await entryRepository.FindAsync<Report>(id, token)
                .ConfigureAwait(false)),
            WorkEntryType.RmpInspection => mapper.Map<InspectionUpdateDto>(await entryRepository
                .FindAsync<RmpInspection>(id, token).ConfigureAwait(false)),
            WorkEntryType.SourceTestReview => mapper.Map<SourceTestReviewUpdateDto>(
                await entryRepository.FindAsync<SourceTestReview>(id, token).ConfigureAwait(false)),

            _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
        };
    }

    public async Task<WorkEntrySummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var entry = mapper.Map<WorkEntrySummaryDto?>(await entryRepository.FindAsync(id, token)
            .ConfigureAwait(false));
        if (entry is null) return entry;
        entry.FacilityName = await facilityService.GetNameAsync((FacilityId)entry.FacilityId).ConfigureAwait(false);
        return entry;
    }

    public async Task<WorkEntryType?> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        await entryRepository.ExistsAsync(id, token).ConfigureAwait(false)
            ? await entryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false)
            : null;

    public async Task<WorkEntryDataSummary> GetDataSummaryAsync(FacilityId facilityId,
        CancellationToken token = default)
    {
        var summary = new WorkEntryDataSummary
        {
            Accs = mapper.Map<IEnumerable<AccViewDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.AnnualComplianceCertification &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            Inspections = mapper.Map<IEnumerable<InspectionViewDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Inspection &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            Notifications = mapper.Map<IEnumerable<NotificationViewDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Notification &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            Reports = mapper.Map<IEnumerable<ReportViewDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Report &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            RmpInspections = mapper.Map<IEnumerable<InspectionViewDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.RmpInspection &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
        };

        // TODO: Implement remaining data summaries.
        //  * EnforcementHistory
        //  * FeesHistory
        //  * SourceTests

        return summary;
    }

    public async Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        await entryRepository.ExistsAsync(referenceNumber, token).ConfigureAwait(false);

    public async Task<SourceTestReviewViewDto?> FindSourceTestReviewAsync(int referenceNumber,
        CancellationToken token = default) =>
        mapper.Map<SourceTestReviewViewDto?>(await entryRepository.FindSourceTestReviewAsync(referenceNumber, token)
            .ConfigureAwait(false));

    // Command
    public async Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = await CreateWorkEntryFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);
        await entryRepository.InsertAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return new CreateResult<int>(workEntry.Id, await appNotificationService
            .SendNotificationAsync(Template.EntryCreated, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false));
    }

    public async Task<AppNotificationResult> UpdateAsync(int id, IWorkEntryCommandDto resource,
        CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token).ConfigureAwait(false);
        workEntry.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await UpdateWorkEntryFromDtoAsync(resource, workEntry, token).ConfigureAwait(false);
        await entryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EntryUpdated, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> CloseAsync(int id, CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Close(workEntry, currentUser);
        await entryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EntryClosed, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> ReopenAsync(int id, CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Reopen(workEntry, currentUser);
        await entryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EntryReopened, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteAsync(int id, StatusCommentDto resource,
        CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Delete(workEntry, resource.Comment, currentUser);
        await entryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EntryDeleted, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token).ConfigureAwait(false);
        entryManager.Restore(workEntry);
        await entryRepository.UpdateAsync(workEntry, autoSave: true, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EntryRestored, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
    }

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(entryRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var workEntry = await entryRepository.GetAsync(itemId, token).ConfigureAwait(false);

        return new CreateResult<Guid>(result.CommentId, await appNotificationService
            .SendNotificationAsync(Template.EntryCommentAdded, workEntry.ResponsibleStaff, token, workEntry.Id,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false));
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(entryRepository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => entryRepository.Dispose();
    public async ValueTask DisposeAsync() => await entryRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
