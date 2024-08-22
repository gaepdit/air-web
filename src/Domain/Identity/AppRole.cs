namespace AirWeb.Domain.Identity;

public static class RoleCategory
{
    public const string General = nameof(General);
    public const string Compliance = nameof(Compliance);
}

/// <summary>
/// Class for listing and describing the application roles for use in the UI, etc.
/// </summary>
public partial class AppRole
{
    public string Name { get; }
    public string Category { get; }
    public string DisplayName { get; }
    public string Description { get; }

    /// <summary>
    /// A Dictionary of all roles used by the app. The Dictionary key is a string containing 
    /// the <see cref="Microsoft.AspNetCore.Identity.IdentityRole.Name"/> of the role.
    /// (This declaration must appear before the list of static instance types.)
    /// </summary>
    public static Dictionary<string, AppRole>? AllRoles { get; private set; }

    private AppRole(string name, string category, string displayName, string description)
    {
        Name = name;
        Category = category;
        DisplayName = displayName;
        Description = description;
        AllRoles ??= new Dictionary<string, AppRole>();
        AllRoles.Add(name, this);
    }

    /// <summary>
    /// A list of all role categories used by the app.
    /// </summary>
    public static List<string> AllRoleCategories { get; } =
    [
        RoleCategory.General,
        RoleCategory.Compliance,
    ];

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
