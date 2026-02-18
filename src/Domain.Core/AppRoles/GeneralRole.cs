namespace AirWeb.Domain.Core.AppRoles;

/// <summary>
/// General User Roles available to the application for authorization.
/// </summary>
public static class GeneralRole
{
    // These are the strings that are stored in the database. Avoid modifying these once published!
    public const string GeneralStaff = nameof(GeneralStaff);
    public const string SiteMaintenance = nameof(SiteMaintenance);
    public const string AppUserAdmin = nameof(AppUserAdmin);

    private const string Category = "General";

    public static void AddRoles() => AppRole.AddAppRoles(GeneralStaffRole, SiteMaintenanceRole, UserAdminRole);

    // The following static Role objects are used for displaying role information in the UI.
    public static AppRole GeneralStaffRole { get; } = new(GeneralStaff, Category, AppRole.RoleType.Staff,
        "General Staff", "Can access search results and read-only views of most data.");

    public static AppRole SiteMaintenanceRole { get; } = new(SiteMaintenance, Category,
        AppRole.RoleType.SiteMaintenance, "Site Maintenance",
        "Can update values in common lookup tables (drop-down lists).");

    public static AppRole UserAdminRole { get; } = new(AppUserAdmin, Category, AppRole.RoleType.UserAdmin,
        "User Account Admin", "Can edit all user profiles and roles.");
}
