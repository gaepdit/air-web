using AirWeb.AppServices.AuthenticationServices.Roles;
using AirWeb.AppServices.DtoInterfaces;
using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.BaseEntities.Interfaces;
using System.Security.Claims;

namespace AirWeb.AppServices.Enforcement.Permissions;

public static class CaseFilePermissions
{
    public static bool CanViewCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        user.CanManageCaseFileDeletions() || !item.IsDeleted && user.IsComplianceStaff() ||
        item.IsClosed && user.IsStaff();

    public static bool CanCloseCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        item is { IsClosed: false, IsDeleted: false } && user.IsEnforcementManager();

    public static bool CanManageCaseFileDeletions(this ClaimsPrincipal user) =>
        user.IsEnforcementManager();

    public static bool CanDeleteCaseFile(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.CanManageCaseFileDeletions();

    public static bool CanEditCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted, IHasOwner =>
        item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanReopenCaseFile<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        item is { IsClosed: true, IsDeleted: false } && user.IsEnforcementManager();

    public static bool CanRestoreCaseFile(this ClaimsPrincipal user, IIsDeleted item) =>
        item.IsDeleted && user.CanManageCaseFileDeletions();
}
