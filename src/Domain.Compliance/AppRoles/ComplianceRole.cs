using AirWeb.Domain.Core.AppRoles;

namespace AirWeb.Domain.Compliance.AppRoles;

/// <summary>
/// Compliance Program User Roles available to the application for authorization.
/// </summary>
public static class ComplianceRole
{
    // These are the strings that are stored in the database. Avoid modifying these once published!
    public const string ComplianceStaff = nameof(ComplianceStaff);
    public const string ComplianceManager = nameof(ComplianceManager);
    public const string EnforcementReviewer = nameof(EnforcementReviewer);
    public const string EnforcementManager = nameof(EnforcementManager);
    public const string ComplianceSiteMaintenance = nameof(ComplianceSiteMaintenance);

    private const string Category = "Compliance and Enforcement";

    public static void AddRoles() => AppRole.AddAppRoles(ComplianceStaffRole, ComplianceManagerRole,
        EnforcementReviewerRole, EnforcementManagerRole, ComplianceSiteMaintenanceRole);

    // The following static Role objects are used for displaying role information in the UI.
    public static AppRole ComplianceStaffRole { get; } = new(ComplianceStaff, Category,
        AppRole.RoleType.Staff, "Compliance Staff", "Can do compliance and enforcement staff work.");

    public static AppRole ComplianceManagerRole { get; } = new(ComplianceManager, Category,
        AppRole.RoleType.Staff, "Compliance Manager", "Can manage compliance work.");

    public static AppRole EnforcementReviewerRole { get; } = new(EnforcementReviewer, Category,
        AppRole.RoleType.Staff, "Enforcement Reviewer", "Can review enforcement actions.");

    public static AppRole EnforcementManagerRole { get; } = new(EnforcementManager, Category,
        AppRole.RoleType.Staff, "Enforcement Manager", "Can resolve enforcement cases.");

    public static AppRole ComplianceSiteMaintenanceRole { get; } = new(ComplianceSiteMaintenance, Category,
        AppRole.RoleType.SiteMaintenance, "Compliance Site Maintenance",
        "Can update values in compliance program lookup tables (drop-down lists).");
}
