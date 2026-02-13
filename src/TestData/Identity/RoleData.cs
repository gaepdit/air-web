using AirWeb.Core.AppRoles;
using AirWeb.Domain.AppRoles;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.TestData.Identity;

internal static partial class UserData
{
    public static List<IdentityUserRole<string>> UserRoles { get; } =
    [
        new()
        {
            UserId = Users[1].Id,
            RoleId = GetRoleId(ComplianceRole.ComplianceManager),
        },
        new()
        {
            UserId = Users[1].Id,
            RoleId = GetRoleId(ComplianceRole.EnforcementManager),
        },
        new()
        {
            UserId = Users[3].Id,
            RoleId = GetRoleId(ComplianceRole.ComplianceStaff),
        },
        new()
        {
            UserId = Users[3].Id,
            RoleId = GetRoleId(ComplianceRole.EnforcementReviewer),
        },
        new()
        {
            UserId = Users[4].Id,
            RoleId = GetRoleId(ComplianceRole.ComplianceStaff),
        },
    ];

    public static List<IdentityRole> Roles
    {
        get
        {
            if (field is not null) return field;
            field = AppRole.AllRoles!
                .Select(pair => new IdentityRole(pair.Value.Name) { NormalizedName = pair.Key.ToUpperInvariant() })
                .ToList();
            return field;
        }
    }

    private static string GetRoleId(string roleName) => Roles.Single(e => e.Name == roleName).Id;
}
