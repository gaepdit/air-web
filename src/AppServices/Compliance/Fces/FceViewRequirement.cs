using AirWeb.AppServices.Compliance.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

internal class FceViewRequirement(IFceService fceService) :
    AuthorizationHandler<ComplianceWorkOperation, FceViewDto>
{
    private FceViewDto _resource = null!;
    private AuthorizationHandlerContext _context = null!;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        FceViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true }) return;

        _resource = resource;
        _context = context;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.AddComment) => ComplianceWorkOperation.CanAddComment(context.User, resource),
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(_context.User, _resource),
            nameof(ComplianceWorkOperation.DeleteComment) => ComplianceWorkOperation.CanDeleteComment(context.User,
                resource),
            nameof(ComplianceWorkOperation.Edit) => ComplianceWorkOperation.CanEdit(_context.User, _resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync().ConfigureAwait(false),
            nameof(ComplianceWorkOperation.ViewDeleted) => ComplianceWorkOperation.CanManageDeletions(_context.User),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync() =>
        ComplianceWorkOperation.CanRestore(_context.User, _resource) &&
        !await fceService.OtherExistsAsync(_resource.Facility.Id, _resource.Year, _resource.Id).ConfigureAwait(false);
}
