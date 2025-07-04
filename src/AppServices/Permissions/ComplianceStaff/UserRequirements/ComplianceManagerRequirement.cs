﻿using Microsoft.AspNetCore.Authorization;

namespace AirWeb.AppServices.Permissions.ComplianceStaff.UserRequirements;

internal class ComplianceManagerRequirement :
    AuthorizationHandler<ComplianceManagerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ComplianceManagerRequirement requirement)
    {
        if (context.User.IsComplianceManager())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
