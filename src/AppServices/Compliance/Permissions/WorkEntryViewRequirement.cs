using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Compliance.Permissions;

internal class WorkEntryViewRequirement :
    AuthorizationHandler<ComplianceWorkOperation, IWorkEntryViewDto>
{
    private IWorkEntryViewDto _resource = null!;
    private AuthorizationHandlerContext _context = null!;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceWorkOperation requirement,
        IWorkEntryViewDto resource)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.FromResult(0);

        _resource = resource;
        _context = context;

        var success = requirement.Name switch
        {
            nameof(ComplianceWorkOperation.Close) => UserCanClose(),
            nameof(ComplianceWorkOperation.Edit) => UserCanEditDetails(),
            nameof(ComplianceWorkOperation.Reopen) => UserCanReopen(),
            nameof(ComplianceWorkOperation.ManageDeletions) => UserCanManageDeletions(),
            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    // Permissions methods
    private bool UserCanClose() => CanCloseOrReopen() && !_resource.IsClosed;
    private bool UserCanReopen() => CanCloseOrReopen() && _resource.IsClosed;

    private bool CanCloseOrReopen() =>
        _context.User.IsComplianceStaff() && _resource is AccViewDto && !_resource.IsDeleted;

    private bool UserCanEditDetails() =>
        _context.User.IsComplianceStaff() && _resource is { IsClosed: false, IsDeleted: false };

    private bool UserCanManageDeletions() => _context.User.IsComplianceManager();
}
