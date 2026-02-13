using AirWeb.AppServices.DtoInterfaces;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace AirWeb.AppServices.AuthorizationPolicies;

public static class PrincipalExtensions
{
    public static bool IsOwner(this ClaimsPrincipal user, IHasOwner item) =>
        item.OwnerId.Equals(user.GetNameIdentifierId());
}
