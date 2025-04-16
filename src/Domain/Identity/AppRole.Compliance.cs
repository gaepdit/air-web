namespace AirWeb.Domain.Identity;

public static partial class RoleName
{
    // These are the strings that are stored in the database. Avoid modifying these once set!

    // Compliance program
    public const string ComplianceStaff = nameof(ComplianceStaff);
    public const string ComplianceManager = nameof(ComplianceManager);
    public const string EnforcementManager = nameof(EnforcementManager);
    public const string ComplianceSiteMaintenance = nameof(ComplianceSiteMaintenance);
}

public partial class AppRole
{
    // These static Role objects are used for displaying role information in the UI.

    [UsedImplicitly]
    public static AppRole ComplianceStaffRole { get; } = new(
        RoleName.ComplianceStaff, RoleCategory.Compliance, "Compliance Staff",
        "Can do compliance and enforcement staff work."
    );

    [UsedImplicitly]
    public static AppRole ComplianceManagerRole { get; } = new(
        RoleName.ComplianceManager, RoleCategory.Compliance, "Compliance Manager",
        "Can manage compliance work."
    );

    [UsedImplicitly]
    public static AppRole EnforcementManagerRole { get; } = new(
        RoleName.EnforcementManager, RoleCategory.Compliance, "Enforcement Manager",
        "Can manage reportable enforcement."
    );

    [UsedImplicitly]
    public static AppRole ComplianceSiteMaintenanceRole { get; } = new(
        RoleName.ComplianceSiteMaintenance, RoleCategory.Compliance, "Compliance Site Maintenance",
        "Can update values in compliance program lookup tables (drop-down lists)."
    );
}
