namespace AirWeb.WebApp.Platform.Settings;

// App-wide global variables
internal static class GlobalConstants
{
    // The date of the final data migration of SSCP compliance and enforcement data from the IAIP.
    public static readonly DateOnly ComplianceDataMigrationDate = new(year: 2026, month: 2, day: 28);
}
