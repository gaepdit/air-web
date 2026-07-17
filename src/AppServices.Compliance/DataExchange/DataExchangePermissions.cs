using AirWeb.AppServices.Compliance.AuthorizationPolicies;
using System.Security.Claims;

namespace AirWeb.AppServices.Compliance.DataExchange;

public static class DataExchangePermissions
{
    public static bool CanRefreshEpaDataExchange(this ClaimsPrincipal user) =>
        user.IsEnforcementManager() || user.IsComplianceManager();
}
