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
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AutoMapper;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;

namespace AirWeb.AppServices.Compliance.WorkEntries;

#pragma warning disable S107 // Methods should not have too many parameters
public sealed partial class WorkEntryService(
    IMapper mapper,
    IComplianceWorkRepository repository,
    IComplianceWorkManager manager,
    IFacilityService facilityService,
    ISourceTestService testService,
    ICommentService<int> commentService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : IWorkEntryService
#pragma warning restore S107
{
    // Query
    public async Task<IWorkEntryViewDto?> FindAsync(int id, bool includeComments, CancellationToken token = default)
    {
        if (!await repository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        IWorkEntryViewDto entry =
            await repository.GetComplianceWorkTypeAsync(id, token).ConfigureAwait(false) switch
            {
                ComplianceWorkType.AnnualComplianceCertification => mapper.Map<AccViewDto>(await repository
                    .FindAsync<AnnualComplianceCertification>(id, includeComments, token).ConfigureAwait(false)),
                ComplianceWorkType.Inspection => mapper.Map<InspectionViewDto>(await repository
                    .FindAsync<Inspection>(id, includeComments, token).ConfigureAwait(false)),
                ComplianceWorkType.Notification => mapper.Map<NotificationViewDto>(await repository
                    .FindAsync<Notification>(id, includeComments, token).ConfigureAwait(false)),
                ComplianceWorkType.PermitRevocation => mapper.Map<PermitRevocationViewDto>(await repository
                    .FindAsync<PermitRevocation>(id, includeComments, token).ConfigureAwait(false)),
                ComplianceWorkType.Report => mapper.Map<ReportViewDto>(await repository
                    .FindAsync<Report>(id, includeComments, token).ConfigureAwait(false)),
                ComplianceWorkType.RmpInspection => mapper.Map<InspectionViewDto>(await repository
                    .FindAsync<RmpInspection>(id, includeComments, token).ConfigureAwait(false)),
                ComplianceWorkType.SourceTestReview => mapper.Map<SourceTestReviewViewDto>(await repository
                    .FindAsync<SourceTestReview>(id, includeComments, token).ConfigureAwait(false)),

                _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Work Entry Type."),
            };

        entry.FacilityName = await facilityService.GetNameAsync((FacilityId)entry.FacilityId).ConfigureAwait(false);
        return entry;
    }

    public async Task<WorkEntrySummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var entry = mapper.Map<WorkEntrySummaryDto?>(await repository.FindAsync(id, token: token)
            .ConfigureAwait(false));
        if (entry is null) return entry;
        entry.FacilityName = await facilityService.GetNameAsync((FacilityId)entry.FacilityId).ConfigureAwait(false);
        return entry;
    }

    public async Task<ComplianceWorkType?> GetWorkEntryTypeAsync(int id, CancellationToken token = default) =>
        await repository.ExistsAsync(id, token).ConfigureAwait(false)
            ? await repository.GetComplianceWorkTypeAsync(id, token).ConfigureAwait(false)
            : null;

    // Enforcement Cases
    public async Task<IEnumerable<int>> GetCaseFileIdsAsync(int id, CancellationToken token = default) =>
        (await repository.FindAsync(entry => entry.Id == id && entry.IsComplianceEvent,
                [nameof(ComplianceEvent.CaseFiles)], token: token)
            .ConfigureAwait(false) as ComplianceEvent)?.CaseFiles.Select(caseFile => caseFile.Id) ?? [];

    // Source test-specific
    public async Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        await repository.SourceTestReviewExistsAsync(referenceNumber, token).ConfigureAwait(false);

    public async Task<SourceTestReviewViewDto?> FindSourceTestReviewAsync(int referenceNumber,
        CancellationToken token = default) =>
        mapper.Map<SourceTestReviewViewDto?>(await repository.FindSourceTestReviewAsync(referenceNumber, token)
            .ConfigureAwait(false));

    // Command
    public async Task<CreateResult<int>> CreateAsync(IWorkEntryCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var workEntry = await CreateWorkEntryFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);
        await repository.InsertAsync(workEntry, token: token).ConfigureAwait(false);

        if (workEntry is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(resource.ResponsibleStaffId!)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryCreated, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CreateResult<int>.Create(workEntry.Id, notificationResult.FailureReason);
    }

    public async Task<CommandResult> UpdateAsync(int id, IWorkEntryCommandDto resource,
        CancellationToken token = default)
    {
        var workEntry = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        await UpdateWorkEntryFromDtoAsync(resource, workEntry, token).ConfigureAwait(false);
        manager.Update(workEntry, currentUser);
        await repository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        if (workEntry is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(resource.ResponsibleStaffId!)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryUpdated, workEntry.ResponsibleStaff, token, id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> CloseAsync(int id, CancellationToken token = default)
    {
        var workEntry = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Close(workEntry, currentUser);
        await repository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        if (workEntry is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryClosed, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> ReopenAsync(int id, CancellationToken token = default)
    {
        var workEntry = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Reopen(workEntry, currentUser);
        await repository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        if (workEntry is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, reviewDate: null)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryReopened, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var workEntry = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Delete(workEntry, resource.Comment, currentUser);
        await repository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        if (workEntry is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, reviewDate: null)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryDeleted, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var workEntry = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Restore(workEntry, currentUser);
        await repository.UpdateAsync(workEntry, token: token).ConfigureAwait(false);

        if (workEntry is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryRestored, workEntry.ResponsibleStaff, token, workEntry.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    // Comments
    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(repository, itemId, resource, token)
            .ConfigureAwait(false);

        // FUTURE: Replace with FindAsync using a query projection.
        var workEntry = await repository.GetAsync(itemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.EntryCommentAdded, workEntry.ResponsibleStaff, token, workEntry.Id,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false);
        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(repository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
