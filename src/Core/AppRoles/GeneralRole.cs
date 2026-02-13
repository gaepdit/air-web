namespace AirWeb.Core.AppRoles;

/// <summary>
/// General User Roles available to the application for authorization.
/// </summary>
public static class GeneralRole
{
    // These are the strings that are stored in the database. Avoid modifying these once published!

    public static readonly string GeneralStaff = nameof(GeneralStaff);
    public static readonly string SiteMaintenance = nameof(SiteMaintenance);
    public static readonly string AppUserAdmin = nameof(AppUserAdmin);

    // (You might be tempted to change the above `static readonly` strings to `const` but don't!
    // The first reference to any of the above causes the following static `AppRole` definitions
    // to be evaluated, which adds them to the common `AppRoles` dictionary.)

    // The following static Role objects are used for displaying role information in the UI.
    private const string General = nameof(General);

    [UsedImplicitly]
    public static AppRole GeneralStaffRole { get; } = new(
        GeneralStaff, General, "General Staff",
        "Can access search results and read-only views of most data.",
        AppRole.SpecializedRole.Staff
    );

    [UsedImplicitly]
    public static AppRole SiteMaintenanceRole { get; } = new(
        SiteMaintenance, General, "Site Maintenance",
        "Can update values in common lookup tables (drop-down lists).",
        AppRole.SpecializedRole.SiteMaintenance
    );

    [UsedImplicitly]
    public static AppRole UserAdminRole { get; } = new(
        AppUserAdmin, General, "User Account Admin",
        "Can edit all user profiles and roles.",
        AppRole.SpecializedRole.UserAdmin
    );
}
