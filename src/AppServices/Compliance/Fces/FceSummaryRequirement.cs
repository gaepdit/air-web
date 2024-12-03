using AirWeb.AppServices.Compliance.Permissions;
using IaipDataService.Facilities;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Fces;

internal class FceSummaryRequirement(IFceService fceService) :
    AuthorizationHandler<ComplianceWorkOperation, FceSummaryDto>
{
    private FceSummaryDto _resource = null!;
    private AuthorizationHandlerContext _context = null!;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        FceSummaryDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true }) return;

        _resource = resource;
        _context = context;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.Delete) => ComplianceWorkOperation.CanDelete(_context.User, _resource),
            nameof(ComplianceWorkOperation.Restore) => await CanRestoreAsync().ConfigureAwait(false),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private async Task<bool> CanRestoreAsync() =>
        ComplianceWorkOperation.CanRestore(_context.User, _resource) &&
        !await fceService.ExistsAsync((FacilityId)_resource.FacilityId, _resource.Year, _resource.Id)
            .ConfigureAwait(false);
}
