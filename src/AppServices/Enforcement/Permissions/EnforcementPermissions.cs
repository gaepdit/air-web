﻿using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class EnforcementPermissions
{
    public static bool CanAddResponse(this IActionViewDto item) =>
        !item.IsDeleted && item is ResponseRequestedViewDto { IsResponseReceived: false };

    public static bool CanEdit(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsCanceled: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanFinalizeAction(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsDeleted: false, IsIssued: false, IsCanceled: false } && user.CanFinalize(item);

    public static bool CanResolve(this ClaimsPrincipal user, IActionViewDto item) =>
        user.CanEdit(item) && item.Status == EnforcementActionStatus.Issued &&
        item is IIsResolved { IsResolved: false };

    public static bool CanSubmitForReview(this ClaimsPrincipal user, IActionViewDto item) =>
        item is { IsDeleted: false, IsCanceled: false, Status: EnforcementActionStatus.Draft } &&
        user.IsComplianceStaff();
}
