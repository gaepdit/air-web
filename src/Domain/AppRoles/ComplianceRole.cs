using AirWeb.Core.AppRoles;

namespace AirWeb.Domain.AppRoles;

/// <summary>
/// Compliance Program User Roles available to the application for authorization.
/// </summary>
public static class ComplianceRole
{
    // These are the strings that are stored in the database. Avoid modifying these once published!

    public static readonly string ComplianceStaff = nameof(ComplianceStaff);
    public static readonly string ComplianceManager = nameof(ComplianceManager);
    public static readonly string EnforcementReviewer = nameof(EnforcementReviewer);
    public static readonly string EnforcementManager = nameof(EnforcementManager);
    public static readonly string ComplianceSiteMaintenance = nameof(ComplianceSiteMaintenance);

    // (You might be tempted to change the above `static readonly` strings to `const` but don't!
    // The first reference to any of the above causes the following static `AppRole` definitions
    // to be evaluated, which adds them to the common `AppRoles` dictionary.)

    // The following static Role objects are used for displaying role information in the UI.
    private const string Compliance = nameof(Compliance);

    [UsedImplicitly]
    public static AppRole ComplianceStaffRole { get; } = new(
        ComplianceStaff, Compliance, "Compliance Staff",
        "Can do compliance and enforcement staff work.",
        AppRole.SpecializedRole.Staff
    );

    [UsedImplicitly]
    public static AppRole ComplianceManagerRole { get; } = new(
        ComplianceManager, Compliance, "Compliance Manager",
        "Can manage compliance work.",
        AppRole.SpecializedRole.Staff
    );

    [UsedImplicitly]
    public static AppRole EnforcementReviewerRole { get; } = new(
        EnforcementReviewer, Compliance, "Enforcement Reviewer",
        "Can review enforcement actions.",
        AppRole.SpecializedRole.Staff
    );

    [UsedImplicitly]
    public static AppRole EnforcementManagerRole { get; } = new(
        EnforcementManager, Compliance, "Enforcement Manager",
        "Can resolve enforcement cases.",
        AppRole.SpecializedRole.Staff
    );

    [UsedImplicitly]
    public static AppRole ComplianceSiteMaintenanceRole { get; } = new(
        ComplianceSiteMaintenance, Compliance, "Compliance Site Maintenance",
        "Can update values in compliance program lookup tables (drop-down lists).",
        AppRole.SpecializedRole.SiteMaintenance
    );
}
