namespace AirWeb.WebApp.Platform.Settings;

// App-wide global variables
internal static class GlobalConstants
{
    // TODO: The date of the final data migration of SSCP compliance and enforcement data from the IAIP.
    public static readonly DateOnly ComplianceDataMigrationDate = new(year: 2025, month: 12, day: 3);
}
