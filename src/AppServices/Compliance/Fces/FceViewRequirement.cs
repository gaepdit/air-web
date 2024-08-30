using AirWeb.AppServices.Permissions.Helpers;
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
            nameof(ComplianceWorkOperation.Edit) => UserCanEdit(),
            nameof(ComplianceWorkOperation.ViewDeleted) => UserCanViewDeleted(),
            nameof(ComplianceWorkOperation.Delete) => UserCanDelete(),
            nameof(ComplianceWorkOperation.Restore) => await UserCanRestoreAsync().ConfigureAwait(false),
            _ => false,
        };

        if (success) context.Succeed(requirement);
    }

    private bool UserCanEdit() => _context.User.IsComplianceStaff() && !_resource.IsDeleted;
    private bool UserCanViewDeleted() => _context.User.IsComplianceManager();
    private bool UserCanDelete() => UserCanViewDeleted() && !_resource.IsDeleted;

    private async Task<bool> UserCanRestoreAsync() =>
        UserCanViewDeleted() && _resource.IsDeleted &&
        !await fceService.ExistsAsync(GetRestoreDto()).ConfigureAwait(false);
}
