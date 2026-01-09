using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace AirWeb.AppServices.AuthenticationServices.Claims;

public static class ClaimsPrincipalExtensions
{
    // Identity Provider claim types
    private const string IdentityProviderId = "idp";
    private const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

    extension(ClaimsPrincipal principal)
    {
        public string? GetIdentityProviderId() =>
            principal.GetClaimValue(IdentityProviderId, TenantId);

        public string? GetAuthenticationMethod() =>
            principal.FindFirstValue(ClaimTypes.AuthenticationMethod);

        public string? GetEmail() =>
            principal.FindFirstValue(ClaimTypes.Email);

        public string GetGivenName() =>
            principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

        public string GetFamilyName() =>
            principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

        public Guid? GetOfficeId()
        {
            var officeIdString = principal.FindFirstValue(AppClaimTypes.OfficeId);
            return officeIdString is null ? null : new Guid(officeIdString);
        }

        public bool HasMatchingClaim(string type,
            [NotNullWhen(true)] string? value) =>
            value is not null && principal.HasClaim(type, value);

        public bool IsActive() =>
            principal.HasClaim(AppClaimTypes.ActiveUser, true.ToString());

        private string? GetClaimValue(params string[] claimNames) => claimNames
            .Select(principal.FindFirstValue)
            .FirstOrDefault(currentValue => !string.IsNullOrEmpty(currentValue));
    }
}
