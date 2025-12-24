using AirWeb.AppServices.AuthenticationServices.Roles;
using AirWeb.AppServices.DtoInterfaces;
using AirWeb.Domain.BaseEntities;
using AirWeb.Domain.BaseEntities.Interfaces;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.Permissions;

public static class CompliancePermissions
{
    // FUTURE: Move these common permissions elsewhere. --
    public static bool CanAddComment(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanClose<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        !item.IsClosed && user.CanFinalize(item);

    public static bool CanDelete(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.CanManageDeletions();

    public static bool CanDeleteComment<T>(this ClaimsPrincipal user, T item)
        where T : IIsDeleted, IHasOwner =>
        !item.IsDeleted && (user.CanManageDeletions() || user.IsOwner(item));
    // -- end common permissions

    public static bool CanEdit<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        item is { IsClosed: false, IsDeleted: false } && user.IsComplianceStaff();

    public static bool CanEdit(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanFinalize(this ClaimsPrincipal user, IIsDeleted item) =>
        !item.IsDeleted && user.IsComplianceStaff();

    public static bool CanManageDeletions(this ClaimsPrincipal user) =>
        user.IsComplianceManager();

    public static bool CanReopen<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        item.IsClosed && user.CanFinalize(item);

    public static bool CanRestore(this ClaimsPrincipal user, IIsDeleted item) =>
        item.IsDeleted && user.CanManageDeletions();

    public static bool CanView<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        user.CanManageDeletions() ||
        !item.IsDeleted && user.IsComplianceStaff() ||
        item.IsClosed && user.IsStaff();

    public static bool CanViewDraftEnforcement<T>(this ClaimsPrincipal user, T item)
        where T : IIsClosed, IIsDeleted =>
        user.IsComplianceStaff();
}
