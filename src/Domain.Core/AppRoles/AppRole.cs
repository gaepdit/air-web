namespace AirWeb.Domain.Core.AppRoles;

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
/// </summary>
public record AppRole(
    string Name,
    string Category,
    AppRole.RoleType Type,
    string DisplayName,
    string Description)
{
    // Add roles to the `AllRoles` Dictionary
    public static void AddAppRoles(params IEnumerable<AppRole> appRoles)
    {
        foreach (var appRole in appRoles)
        {
            AllRoleCategories.Add(appRole.Category);
            AllRoles.TryAdd(appRole.Name, appRole);
            SpecializedRoles[appRole.Type].Add(appRole.Name);
        }
    }

    /// <summary>
    /// A list of all role categories used by the app.
    /// </summary>
    public static HashSet<string> AllRoleCategories { get; } = [];

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// </summary>
    public static Dictionary<string, AppRole> AllRoles { get; } = new();

    // Specialized Role sets
    public enum RoleType
    {
        SiteMaintenance,
        Staff,
        UserAdmin,
    }

    public static HashSet<string> SiteMaintenanceRoles { get; } = [];
    public static HashSet<string> StaffRoles { get; } = [];
    public static HashSet<string> UserAdminRoles { get; } = [];

    public static Dictionary<RoleType, HashSet<string>> SpecializedRoles { get; } = new()
    {
        [RoleType.SiteMaintenance] = SiteMaintenanceRoles,
        [RoleType.Staff] = StaffRoles,
        [RoleType.UserAdmin] = UserAdminRoles,
    };

    /// <summary>
    /// Converts a list of role strings to a list of <see cref="AppRole"/> objects.
    /// </summary>
    /// <param name="roles">A list of role strings.</param>
    /// <returns>A list of AppRoles.</returns>
    public static IEnumerable<AppRole> RolesAsAppRoles(IEnumerable<string> roles)
    {
        var appRoles = new List<AppRole>();

        foreach (var role in roles)
            if (AllRoles.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }
}
