namespace AirWeb.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;
    public bool StartsWithVowelSound { get; private init; }

    private MaintenanceOption() { }

    public static MaintenanceOption NotificationType { get; } =
        new()
        {
            SingularName = "Compliance Notification Type",
            PluralName = "Compliance Notification Types",
            StartsWithVowelSound = false,
        };

    public static MaintenanceOption Office { get; } =
        new()
        {
            SingularName = "Office",
            PluralName = "Offices",
            StartsWithVowelSound = true,
        };
}
