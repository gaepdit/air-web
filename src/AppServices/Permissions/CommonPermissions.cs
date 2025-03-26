using AirWeb.AppServices.CommonInterfaces;
using AirWeb.AppServices.Permissions.Helpers;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Permissions;

public static class CommonPermissions
{
    public static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());

    public static bool IsOwnerOrManager(this ClaimsPrincipal user, IHasOwner item) =>
        user.IsOwner(item) || user.IsComplianceManager();
}
