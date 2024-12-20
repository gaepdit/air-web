using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.Command;
using AirWeb.AppServices.Enforcement.Query;
using AirWeb.AppServices.Users;
using AirWeb.Domain.EnforcementEntities;
using AutoMapper;
using FluentValidation;
using IaipDataService.Facilities;
using ValidationException = FluentValidation.ValidationException;

namespace AirWeb.AppServices.Enforcement;

public class EnforcementService(
    IMapper mapper,
    ICaseFileRepository caseFileRepository,
    ICommentService<int> commentService,
    IFacilityService facilityService,
    IUserService userService,
    IValidator<CaseFileUpdateDto> updateValidator,
    IAppNotificationService appNotificationService)
    : IEnforcementService
{
    public async Task<CaseFileViewDto?> FindDetailedCaseFileAsync(int id, CancellationToken token = default)
    {
        var caseFile = mapper.Map<CaseFileViewDto?>(await caseFileRepository.FindDetailedCaseFileAsync(id, token)
            .ConfigureAwait(false));

        if (caseFile != null)
        {
            caseFile.FacilityName = await facilityService.GetNameAsync((FacilityId)caseFile.FacilityId)
                .ConfigureAwait(false);
        }

        return caseFile;
    }

    public async Task<CaseFileSummaryDto?> FindCaseFileSummaryAsync(int id, CancellationToken token = default)
    {
        var caseFile = mapper.Map<CaseFileSummaryDto?>(await caseFileRepository.FindAsync(id, token)
            .ConfigureAwait(false));

        if (caseFile != null)
        {
            caseFile.FacilityName = await facilityService.GetNameAsync((FacilityId)caseFile.FacilityId)
                .ConfigureAwait(false);
        }

        return caseFile;
    }

    public async Task<AppNotificationResult> UpdateCaseFileAsync(int id, CaseFileUpdateDto resource,
        CancellationToken token)
    {
        var validationResult = await updateValidator.ValidateAsync(resource, token).ConfigureAwait(false);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var caseFile = await caseFileRepository.GetAsync(id, token).ConfigureAwait(false);
        caseFile.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        // Update the case file properties
        caseFile.ResponsibleStaff = await userService.FindUserAsync(resource.ResponsibleStaffId).ConfigureAwait(false);
        caseFile.DiscoveryDate = resource.DiscoveryDate;
        caseFile.Notes = resource.Notes;

        await caseFileRepository.UpdateAsync(caseFile, token: token).ConfigureAwait(false);

        return await appNotificationService
            .SendNotificationAsync(Template.EnforcementUpdated, caseFile.ResponsibleStaff, token, id)
            .ConfigureAwait(false);
    }

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(caseFileRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var caseFile = await caseFileRepository.GetAsync(resource.ItemId, token).ConfigureAwait(false);

        return new CreateResult<Guid>(result.CommentId, await appNotificationService
            .SendNotificationAsync(Template.EnforcementCommentAdded, caseFile.ResponsibleStaff, token, itemId,
                resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false));
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(caseFileRepository, commentId, token);
}
