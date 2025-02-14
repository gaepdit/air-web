using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AutoMapper;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Fces;

public sealed class FceService(
    IMapper mapper,
    IFceRepository fceRepository,
    IFceManager fceManager,
    IWorkEntryRepository entryRepository,
    IFacilityService facilityService,
    ICommentService<int> commentService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : IFceService
{
    public async Task<FceViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var fce = mapper.Map<FceViewDto?>(await fceRepository.FindWithCommentsAsync(id, token).ConfigureAwait(false));
        if (fce is null) return null;
        fce.FacilityName = await facilityService.GetNameAsync((FacilityId)fce.FacilityId).ConfigureAwait(false);
        return fce;
    }

    public async Task<FceSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var fce = mapper.Map<FceSummaryDto?>(await fceRepository.FindAsync(id, token).ConfigureAwait(false));
        if (fce is null) return null;
        fce.FacilityName = await facilityService.GetNameAsync((FacilityId)fce.FacilityId).ConfigureAwait(false);
        return fce;
    }

    public async Task<SupportingDataSummary> GetSupportingDataAsync(FacilityId facilityId,
        CancellationToken token = default)
    {
        var summary = new SupportingDataSummary
        {
            Accs = mapper.Map<IEnumerable<AccSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.AnnualComplianceCertification &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            Inspections = mapper.Map<IEnumerable<InspectionSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Inspection &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            Notifications = mapper.Map<IEnumerable<NotificationSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Notification &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            Reports = mapper.Map<IEnumerable<ReportSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Report &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
            RmpInspections = mapper.Map<IEnumerable<InspectionSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.RmpInspection &&
                         entry.FacilityId == facilityId, token).ConfigureAwait(false)),
        };

        // TODO: Implement remaining data summaries.
        //  * EnforcementHistory
        //  * FeesHistory
        //  * SourceTests

        return summary;
    }

    public async Task<NotificationResultWithId<int>> CreateAsync(FceCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var fce = await fceManager.CreateAsync((FacilityId)resource.FacilityId!, resource.Year, currentUser, token)
            .ConfigureAwait(false);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        await fceRepository.InsertAsync(fce, token: token).ConfigureAwait(false);

        return new NotificationResultWithId<int>(fce.Id, await appNotificationService
            .SendNotificationAsync(Template.FceCreated, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false));
    }

    public async Task<AppNotificationResult> UpdateAsync(int id, FceUpdateDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token).ConfigureAwait(false);
        fce.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.FceUpdated, fce.ReviewedBy, token, id)
            .ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Delete(fce, resource.Comment, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.FceDeleted, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token).ConfigureAwait(false);
        fceManager.Restore(fce);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.FceRestored, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
    }

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int currentId, CancellationToken token = default) =>
        fceRepository.ExistsAsync(facilityId, year, currentId, token);

    public async Task<NotificationResultWithId<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(fceRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var fce = await fceRepository.GetAsync(resource.ItemId, token).ConfigureAwait(false);

        return new NotificationResultWithId<Guid>(result.CommentId, await appNotificationService
            .SendNotificationAsync(Template.FceCommentAdded, fce.ReviewedBy, token, itemId,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false));
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(fceRepository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => fceRepository.Dispose();
    public async ValueTask DisposeAsync() => await fceRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
