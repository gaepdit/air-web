namespace AirWeb.Core.AppRoles;

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
/// </summary>
public class AppRole
{
    public string Name { get; }
    public string Category { get; }
    public string DisplayName { get; }
    public string Description { get; }

    public AppRole(string name, string category, string displayName, string description,
        SpecializedRole? specializedRole = null)
    {
        Name = name;
        Category = category;
        AllRoleCategories.Add(category);
        DisplayName = displayName;
        Description = description;
        AllRoles.Add(name, this);
        if (specializedRole != null) SpecializedRoles[specializedRole.Value].Add(name);
    }

    /// <summary>
    /// A list of all role categories used by the app.
    /// </summary>
    public static HashSet<string> AllRoleCategories { get; } = [];

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// (This declaration must appear before the list of static instance types.)
    /// </summary>
    public static Dictionary<string, AppRole> AllRoles { get; } = new();

    // Specialized Role sets
    public enum SpecializedRole
    {
        SiteMaintenance,
        Staff,
        UserAdmin,
    }

    public static HashSet<string> SiteMaintenanceRoles { get; } = [];
    public static HashSet<string> StaffRoles { get; } = [];
    public static HashSet<string> UserAdminRoles { get; } = [];

    public static Dictionary<SpecializedRole, HashSet<string>> SpecializedRoles { get; } = new()
    {
        [SpecializedRole.SiteMaintenance] = SiteMaintenanceRoles,
        [SpecializedRole.Staff] = StaffRoles,
        [SpecializedRole.UserAdmin] = UserAdminRoles,
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
            if (AllRoles!.TryGetValue(role, out var appRole))
                appRoles.Add(appRole);

        return appRoles;
    }
}
