﻿using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ValueObjects;
using AutoMapper;

namespace AirWeb.AppServices.Compliance.Fces;

public sealed class FceService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IFceRepository fceRepository,
    IFceManager fceManager,
    IAppNotificationService appNotificationService)
    : IFceService
{
    public async Task<FceViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.FindAsync(id, token).ConfigureAwait(false);
        if (fce is null) return null;
        await fceManager.LoadFacilityAsync(fce, token).ConfigureAwait(false);
        return mapper.Map<FceViewDto>(fce);
    }

    public async Task<FceUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<FceUpdateDto?>(await fceRepository.FindAsync(fce => fce.Id.Equals(id) && !fce.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task<CreateResult<int>> CreateAsync(FceCreateDto resource, CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var fce = await fceManager.CreateAsync(resource.FacilityId!, resource.Year, currentUser, token)
            .ConfigureAwait(false);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById).ConfigureAwait(false);
        fce.CompletedDate = resource.CompletedDate;
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes;

        await fceRepository.InsertAsync(fce, token: token).ConfigureAwait(false);

        return new CreateResult<int>(fce.Id,
            await NotifyOwnerAsync(fce, Template.FceCreated, token).ConfigureAwait(false));
    }

    public async Task<AppNotificationResult> UpdateAsync(int id, FceUpdateDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token).ConfigureAwait(false);
        fce.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById).ConfigureAwait(false);
        fce.CompletedDate = resource.CompletedDate;
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes;

        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(fce, Template.FceUpdated, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var comment = Comment.CreateComment(resource.Comment, currentUser);
        await fceRepository.AddCommentAsync(id, comment, token).ConfigureAwait(false);

        var fce = await fceRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        return await NotifyOwnerAsync(fce, Template.FceCommentAdded, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(resource.Id, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Delete(fce, resource.Comment, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(fce, Template.FceDeleted, token).ConfigureAwait(false);
    }

    public async Task<AppNotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(resource.Id, token).ConfigureAwait(false);

        fceManager.Restore(fce);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);
        return await NotifyOwnerAsync(fce, Template.FceRestored, token).ConfigureAwait(false);
    }

    private async Task<AppNotificationResult> NotifyOwnerAsync(Fce fce, Template template,
        CancellationToken token, Comment? comment = null)
    {
        var recipient = fce.ReviewedBy;

        if (recipient is null)
            return AppNotificationResult.NotAttemptedResult();
        if (!recipient.Active)
            return AppNotificationResult.FailureResult("The recipient is not an active user.");
        if (recipient.Email is null)
            return AppNotificationResult.FailureResult("The recipient cannot be emailed.");

        return comment is null
            ? await appNotificationService.SendNotificationAsync(template, recipient.Email, token, fce.Id.ToString())
                .ConfigureAwait(false)
            : await appNotificationService.SendNotificationAsync(template, recipient.Email, token, fce.Id.ToString(),
                    comment.Text, comment.CommentedAt.ToString(), comment.CommentBy?.FullName)
                .ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => fceRepository.Dispose();
    public async ValueTask DisposeAsync() => await fceRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
