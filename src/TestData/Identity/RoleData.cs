using AirWeb.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.TestData.Identity;

internal static partial class UserData
{
    public static List<IdentityUserRole<string>> UserRoles { get; } =
    [
        new()
        {
            UserId = Users[1].Id,
            RoleId = GetRoleId(RoleName.ComplianceManager),
        },
        new()
        {
            UserId = Users[1].Id,
            RoleId = GetRoleId(RoleName.EnforcementManager),
        },
        new()
        {
            UserId = Users[3].Id,
            RoleId = GetRoleId(RoleName.ComplianceStaff),
        },
        new()
        {
            UserId = Users[3].Id,
            RoleId = GetRoleId(RoleName.EnforcementReviewer),
        },
        new()
        {
            UserId = Users[4].Id,
            RoleId = GetRoleId(RoleName.ComplianceStaff),
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
