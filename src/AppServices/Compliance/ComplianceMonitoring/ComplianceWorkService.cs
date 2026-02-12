using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Query;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Inspections;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.Reports;
using AirWeb.AppServices.Compliance.ComplianceMonitoring.SourceTestReviews;
using AirWeb.AppServices.Enforcement;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AutoMapper;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring;

#pragma warning disable S107 // Methods should not have too many parameters
public sealed partial class ComplianceWorkService(
    IMapper mapper,
    IComplianceWorkRepository repository,
    IComplianceWorkManager manager,
    IFacilityService facilityService,
    ISourceTestService testService,
    IComplianceWorkCommentService commentService,
    IUserService userService,
    ICaseFileService caseFileService,
    IAppNotificationService appNotificationService)
    : IComplianceWorkService
#pragma warning restore S107
{
    // Query
    public async Task<IComplianceWorkViewDto?> FindAsync(int id, bool includeComments,
        CancellationToken token = default)
    {
        if (!await repository.ExistsAsync(id, token).ConfigureAwait(false)) return null;

        IComplianceWorkViewDto work =
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

                _ => throw new ArgumentOutOfRangeException(nameof(id), "Item has an invalid Compliance Work Type."),
            };

        work.FacilityName = await facilityService.GetNameAsync((FacilityId)work.FacilityId).ConfigureAwait(false);
        return work;
    }

    public async Task<ComplianceWorkSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var work = mapper.Map<ComplianceWorkSummaryDto?>(await repository.FindAsync(id, token: token)
            .ConfigureAwait(false));
        if (work is null) return work;
        work.FacilityName = await facilityService.GetNameAsync((FacilityId)work.FacilityId).ConfigureAwait(false);
        return work;
    }

    public async Task<ComplianceWorkType?> GetComplianceWorkTypeAsync(int id, CancellationToken token = default) =>
        await repository.ExistsAsync(id, token).ConfigureAwait(false)
            ? await repository.GetComplianceWorkTypeAsync(id, token).ConfigureAwait(false)
            : null;

    // Enforcement Cases
    public async Task<IEnumerable<int>> GetCaseFileIdsAsync(int id, CancellationToken token = default) =>
        (await repository.FindAsync(work => work.Id == id && work.IsComplianceEvent,
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
    public async Task<CreateResult<int>> CreateAsync(IComplianceWorkCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var work = await CreateComplianceWorkFromDtoAsync(resource, currentUser, token).ConfigureAwait(false);
        await repository.InsertAsync(work, token: token).ConfigureAwait(false);

        if (work is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(resource.ResponsibleStaffId!)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkCreated, work.ResponsibleStaff, token, work.Id)
            .ConfigureAwait(false);
        return CreateResult<int>.Create(work.Id, notificationResult.FailureReason);
    }

    public async Task<CommandResult> UpdateAsync(int id, IComplianceWorkCommandDto resource,
        CancellationToken token = default)
    {
        var work = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        await UpdateComplianceWorkFromDtoAsync(resource, work, token).ConfigureAwait(false);
        manager.Update(work, currentUser);
        await repository.UpdateAsync(work, token: token).ConfigureAwait(false);

        if (work is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(resource.ResponsibleStaffId!)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkUpdated, work.ResponsibleStaff, token, id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> CloseAsync(int id, CancellationToken token = default)
    {
        var work = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Close(work, currentUser);
        await repository.UpdateAsync(work, token: token).ConfigureAwait(false);

        if (work is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkClosed, work.ResponsibleStaff, token, work.Id)
            .ConfigureAwait(false);

        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> ReopenAsync(int id, CancellationToken token = default)
    {
        var work = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Reopen(work, currentUser);
        await repository.UpdateAsync(work, token: token).ConfigureAwait(false);

        if (work is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, reviewDate: null)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkReopened, work.ResponsibleStaff, token, work.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var work = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Delete(work, resource.Comment, currentUser);
        await repository.UpdateAsync(work, autoSave: false, token: token).ConfigureAwait(false);

        if (work is ComplianceEvent ce)
        {
            var caseFiles = await GetCaseFileIdsAsync(id, token).ConfigureAwait(false);
            foreach (var caseFile in caseFiles)
                await caseFileService.UnLinkComplianceEventAsync(caseFile, ce.Id, autoSave: false, token: token)
                    .ConfigureAwait(false);
        }

        await repository.SaveChangesAsync(token).ConfigureAwait(false);

        if (work is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, reviewDate: null)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkDeleted, work.ResponsibleStaff, token, work.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var work = await repository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        manager.Restore(work, currentUser);
        await repository.UpdateAsync(work, token: token).ConfigureAwait(false);

        if (work is SourceTestReview str)
        {
            var complianceEmail = await userService.GetUserEmailAsync(str.ResponsibleStaff!.Id)
                .ConfigureAwait(false);
            await testService.UpdateSourceTestAsync(str.ReferenceNumber!.Value, complianceEmail!, str.ClosedDate)
                .ConfigureAwait(false);
        }

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkRestored, work.ResponsibleStaff, token, work.Id)
            .ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    // Comments
    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(itemId, resource, token)
            .ConfigureAwait(false);

        // FUTURE: Replace with FindAsync using a query projection.
        var work = await repository.GetAsync(itemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.WorkCommentAdded, work.ResponsibleStaff, token, work.Id,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false);
        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
