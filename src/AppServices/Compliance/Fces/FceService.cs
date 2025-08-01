﻿using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AutoMapper;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Fces;

#pragma warning disable S107 // Methods should not have too many parameters
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
#pragma warning restore S107
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
        var fce = mapper.Map<FceSummaryDto?>(await fceRepository.FindAsync(id, token: token).ConfigureAwait(false));
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
                         entry.FacilityId == facilityId, token: token).ConfigureAwait(false)),
            Inspections = mapper.Map<IEnumerable<InspectionSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Inspection &&
                         entry.FacilityId == facilityId, token: token).ConfigureAwait(false)),
            Notifications = mapper.Map<IEnumerable<NotificationSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Notification &&
                         entry.FacilityId == facilityId, token: token).ConfigureAwait(false)),
            Reports = mapper.Map<IEnumerable<ReportSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.Report &&
                         entry.FacilityId == facilityId, token: token).ConfigureAwait(false)),
            RmpInspections = mapper.Map<IEnumerable<InspectionSummaryDto>>(await entryRepository.GetListAsync(
                entry => entry.WorkEntryType == WorkEntryType.RmpInspection &&
                         entry.FacilityId == facilityId, token: token).ConfigureAwait(false)),
        };

        // TODO: Implement remaining data summaries.
        //  * EnforcementHistory
        //  * FeesHistory
        //  * SourceTests

        return summary;
    }

    public async Task<CreateResult<int>> CreateAsync(FceCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var fce = await fceManager.CreateAsync((FacilityId)resource.FacilityId!, resource.Year, currentUser, token)
            .ConfigureAwait(false);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById!).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        await fceRepository.InsertAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceCreated, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);

        return CreateResult<int>.Create(fce.Id, notificationResult.FailureReason);
    }

    public async Task<CommandResult> UpdateAsync(int id, FceUpdateDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        fce.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById!).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceUpdated, fce.ReviewedBy, token, id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Delete(fce, resource.Comment, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceDeleted, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        fceManager.Restore(fce);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceRestored, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int currentId, CancellationToken token = default) =>
        fceRepository.ExistsAsync(facilityId, year, currentId, token);

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(fceRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var fce = await fceRepository.GetAsync(resource.ItemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService.SendNotificationAsync(Template.FceCommentAdded,
            fce.ReviewedBy, token, itemId, resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false);
        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(fceRepository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => fceRepository.Dispose();
    public async ValueTask DisposeAsync() => await fceRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
