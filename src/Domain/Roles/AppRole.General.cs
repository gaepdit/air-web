namespace AirWeb.Domain.Roles;

/// <summary>
/// User Roles available to the application for authorization.
/// </summary>
public static partial class RoleName
{
    // These are the strings that are stored in the database. Avoid modifying these once set!

    // General
    public const string GeneralStaff = nameof(GeneralStaff);
    public const string SiteMaintenance = nameof(SiteMaintenance);
    public const string AppUserAdmin = nameof(AppUserAdmin);
}

public partial class AppRole
{
    // These static Role objects are used for displaying role information in the UI.

    [UsedImplicitly]
    public static AppRole GeneralStaffRole { get; } = new(
        RoleName.GeneralStaff, RoleCategory.General, "General Staff",
        "Can access search results and read-only views of most data."
    );

    [UsedImplicitly]
    public static AppRole SiteMaintenanceRole { get; } = new(
        RoleName.SiteMaintenance, RoleCategory.General, "Site Maintenance",
        "Can update values in common lookup tables (drop-down lists)."
    );

    [UsedImplicitly]
    public static AppRole UserAdminRole { get; } = new(
        RoleName.AppUserAdmin, RoleCategory.General, "User Account Admin",
        "Can edit all user profiles and roles."
    );
}
