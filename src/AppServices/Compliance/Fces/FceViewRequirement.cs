using AirWeb.AppServices.Compliance.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

internal class FceViewRequirement(IFceService fceService) :
    AuthorizationHandler<ComplianceWorkOperation, FceViewDto>
{
    private FceViewDto _resource = null!;
    private AuthorizationHandlerContext _context = null!;
    private FceRestoreDto GetRestoreDto() => new(_resource.Id, _resource.Facility.Id, _resource.Year);

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
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(_context.User, _resource),
            nameof(ComplianceWorkOperation.Edit) => ComplianceWorkOperation.CanEdit(_context.User, _resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync().ConfigureAwait(false),
            nameof(ComplianceWorkOperation.ViewDeleted) => ComplianceWorkOperation.CanManageDeletions(_context.User),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync() =>
        ComplianceWorkOperation.CanRestore(_context.User, _resource) &&
        !await fceService.ExistsAsync(GetRestoreDto()).ConfigureAwait(false);
}
