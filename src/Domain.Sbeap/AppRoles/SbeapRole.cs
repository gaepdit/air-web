using AirWeb.Domain.Core.AppRoles;

namespace AirWeb.Domain.Sbeap.AppRoles;

/// <summary>
/// SBEAP User Roles available to the application for authorization.
/// </summary>
public static class SbeapRole
{
    // These are the strings that are stored in the database. Avoid modifying these once published!
    public const string SbeapAdmin = nameof(SbeapAdmin);
    public const string SbeapStaff = nameof(SbeapStaff);
    public const string SbeapSiteMaintenance = nameof(SbeapSiteMaintenance);

    private const string Category = "SBEAP";

    public static void AddRoles() => AppRole.AddAppRoles(SbeapAdminRole, SbeapSiteMaintenanceRole, SbeapStaffRole);

    // The following static Role objects are used for displaying role information in the UI.
    private static AppRole SbeapAdminRole { get; } = new(SbeapAdmin, Category,
        AppRole.RoleType.UserAdmin, "SBEAP Admin",
        "Can manage SBEAP work.");

    private static AppRole SbeapStaffRole { get; } = new(SbeapStaff, Category,
        AppRole.RoleType.Staff, "SBEAP Staff",
        "Can do compliance and enforcement staff work.");

    private static AppRole SbeapSiteMaintenanceRole { get; } = new(SbeapSiteMaintenance, Category,
        AppRole.RoleType.SiteMaintenance, "SBEAP Site Maintenance",
        "Can update values in SBEAP program lookup tables (drop-down lists).");
}
