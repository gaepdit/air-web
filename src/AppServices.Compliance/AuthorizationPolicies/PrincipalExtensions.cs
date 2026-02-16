using AirWeb.AppServices.Compliance.DtoInterfaces;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.AuthorizationPolicies;

public static class PrincipalExtensions
{
    public static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
