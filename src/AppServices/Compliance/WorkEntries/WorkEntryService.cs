using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
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
    public async Task<IWorkEntryViewDto?> FindAsync(int id, bool includeComments, CancellationToken token = default)
    {
        if (!await entryRepository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        IWorkEntryViewDto entry =
            await entryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false) switch
            {
                WorkEntryType.AnnualComplianceCertification => mapper.Map<AccViewDto>(await entryRepository
                    .FindAsync<AnnualComplianceCertification>(id, includeComments, token).ConfigureAwait(false)),
                WorkEntryType.Inspection => mapper.Map<InspectionViewDto>(await entryRepository
                    .FindAsync<Inspection>(id, includeComments, token).ConfigureAwait(false)),
                WorkEntryType.Notification => mapper.Map<NotificationViewDto>(await entryRepository
                    .FindAsync<Notification>(id, includeComments, token).ConfigureAwait(false)),
                WorkEntryType.PermitRevocation => mapper.Map<PermitRevocationViewDto>(await entryRepository
                    .FindAsync<PermitRevocation>(id, includeComments, token).ConfigureAwait(false)),
                WorkEntryType.Report => mapper.Map<ReportViewDto>(await entryRepository
                    .FindAsync<Report>(id, includeComments, token).ConfigureAwait(false)),
                WorkEntryType.RmpInspection => mapper.Map<InspectionViewDto>(await entryRepository
                    .FindAsync<RmpInspection>(id, includeComments, token).ConfigureAwait(false)),
                WorkEntryType.SourceTestReview => mapper.Map<SourceTestReviewViewDto>(await entryRepository
                    .FindAsync<SourceTestReview>(id, includeComments, token).ConfigureAwait(false)),

                _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
            };

        entry.FacilityName = await facilityService.GetNameAsync((FacilityId)entry.FacilityId).ConfigureAwait(false);
        return entry;
    }

    public async Task<WorkEntrySummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var entry = mapper.Map<WorkEntrySummaryDto?>(await entryRepository.FindAsync(id, token: token)
            .ConfigureAwait(false));
        if (entry is null) return entry;
        entry.FacilityName = await facilityService.GetNameAsync((FacilityId)entry.FacilityId).ConfigureAwait(false);
        return entry;
    }

    public async Task<WorkEntryType?> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        await entryRepository.ExistsAsync(id, token).ConfigureAwait(false)
            ? await entryRepository.GetWorkEntryTypeAsync(id, token).ConfigureAwait(false)
            : null;

    // Enforcement Cases
    public async Task<IEnumerable<int>> GetCaseFileIdsAsync(int id, CancellationToken token = default) =>
        (await entryRepository.FindAsync(entry => entry.Id == id && entry.IsComplianceEvent, token: token)
            .ConfigureAwait(false) as ComplianceEvent)?.CaseFiles.Select(caseFile => caseFile.Id) ?? [];

    // Source test-specific
    public async Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        await entryRepository.SourceTestReviewExistsAsync(referenceNumber, token).ConfigureAwait(false);

    public async Task<SourceTestReviewViewDto?> FindSourceTestReviewAsync(int referenceNumber,
        CancellationToken token = default) =>
        mapper.Map<SourceTestReviewViewDto?>(await entryRepository.FindSourceTestReviewAsync(referenceNumber, token)
            .ConfigureAwait(false));

    // Command
    public async Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = await CreateWorkEntryFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);
        await entryRepository.InsertAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryCreated, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CreateResult<int>.Create(workEntry.Id, notificationResult.FailureReason);
    }

    public async Task<CommandResult> UpdateAsync(int id, IWorkEntryCommandDto resource,
        CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        await UpdateWorkEntryFromDtoAsync(resource, workEntry, token).ConfigureAwait(false);
        entryManager.Update(workEntry, currentUser);
        await entryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryUpdated, workEntry.ResponsibleStaff, token, id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> CloseAsync(int id, CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Close(workEntry, currentUser);
        await entryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryClosed, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> ReopenAsync(int id, CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Reopen(workEntry, currentUser);
        await entryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryReopened, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Delete(workEntry, resource.Comment, currentUser);
        await entryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryDeleted, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var workEntry = await entryRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        entryManager.Restore(workEntry, currentUser);
        await entryRepository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryRestored, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    // Comments
    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(entryRepository, itemId, resource, token)
            .ConfigureAwait(false);

        // TODO: Replace with FindAsync using a query projection.
        var workEntry = await entryRepository.GetAsync(itemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryCommentAdded, workEntry.ResponsibleStaff, token, workEntry.Id,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false);
        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(entryRepository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => entryRepository.Dispose();
    public async ValueTask DisposeAsync() => await entryRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
