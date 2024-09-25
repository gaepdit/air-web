using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Users;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.Domain.Identity;
using AutoMapper;

namespace AirWeb.AppServices.Compliance.Fces;

public sealed class FceService(
    IMapper mapper,
    IFceRepository fceRepository,
    IFceManager fceManager,
    ICommentService<int> commentService,
    IUserService userService,
    IAppNotificationService appNotificationService)
    : IFceService
{
    public async Task<FceViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.FindWithCommentsAsync(id, token).ConfigureAwait(false);
        if (fce is null) return null;
        await fceManager.LoadFacilityAsync(fce, token).ConfigureAwait(false);
        return mapper.Map<FceViewDto>(fce);
    }

    public async Task<FceUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<FceUpdateDto?>(await fceRepository.FindAsync(fce => fce.Id.Equals(id) && !fce.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task<FceSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.FindAsync(id, token).ConfigureAwait(false);
        if (fce is null) return null;
        await fceManager.LoadFacilityAsync(fce, token).ConfigureAwait(false);
        return mapper.Map<FceSummaryDto>(fce);
    }

    public async Task<CreateResult<int>> CreateAsync(FceCreateDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var fce = await fceManager.CreateAsync((FacilityId)resource.FacilityId!, resource.Year, currentUser, token)
            .ConfigureAwait(false);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        await fceRepository.InsertAsync(fce, token: token).ConfigureAwait(false);

        return new CreateResult<int>(fce.Id,
            await NotifyOwnerAsync(fce.Id, fce.ReviewedBy, Template.FceCreated, token).ConfigureAwait(false));
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
        return await NotifyOwnerAsync(fce.Id, fce.ReviewedBy, Template.FceUpdated, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteAsync(int id, StatusCommentDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Delete(fce, resource.Comment, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(fce.Id, fce.ReviewedBy, Template.FceDeleted, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token).ConfigureAwait(false);
        fceManager.Restore(fce);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(fce.Id, fce.ReviewedBy, Template.FceRestored, token).ConfigureAwait(false);
    }

    public Task<bool> OtherExistsAsync(FacilityId facilityId, int year, int currentId,
        CancellationToken token = default) =>
        fceRepository.ExistsAsync(facilityId, year, currentId, token);

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(fceRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var fce = await fceRepository.GetAsync(resource.ItemId, token).ConfigureAwait(false);
        return new CreateResult<Guid>(result.CommentId,
            await NotifyOwnerAsync(fce.Id, fce.ReviewedBy, Template.FceCommentAdded, token, resource.Comment,
                result.CommentUser?.FullName).ConfigureAwait(false));
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(fceRepository, commentId, token);

    private async Task<AppNotificationResult> NotifyOwnerAsync(int fceId, ApplicationUser? recipient, Template template,
        CancellationToken token, string? comment = null, string? commentBy = null)
    {
        if (recipient is null)
            return AppNotificationResult.NotAttemptedResult();
        if (!recipient.Active)
            return AppNotificationResult.FailureResult("The recipient is not an active user.");
        if (recipient.Email is null)
            return AppNotificationResult.FailureResult("The recipient cannot be emailed.");

        return comment is null
            ? await appNotificationService
                .SendNotificationAsync(template, recipient.Email, token, fceId.ToString())
                .ConfigureAwait(false)
            : await appNotificationService
                .SendNotificationAsync(template, recipient.Email, token, fceId.ToString(), comment, commentBy)
                .ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => fceRepository.Dispose();
    public async ValueTask DisposeAsync() => await fceRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
