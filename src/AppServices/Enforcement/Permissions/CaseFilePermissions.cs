using AirWeb.AppServices.DtoInterfaces;
using AirWeb.AppServices.Permissions.ComplianceStaff;
using AirWeb.Domain.BaseEntities.Interfaces;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class CaseFilePermissions
{
    public static bool CanCloseCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        item is { IsClosed: false, IsDeleted: false } && user.IsEnforcementManager();

    public static bool CanDeleteCaseFile(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsEnforcementManager();

    public static bool CanEditCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted, IHasOwner =>
        item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanReopenCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        item is { IsClosed: true, IsDeleted: false } && user.IsEnforcementManager();

    public static bool CanRestoreCaseFile(this ClaimsPrincipal user, IIsDeleted item) =>
        item.IsDeleted && user.IsEnforcementManager();
}
