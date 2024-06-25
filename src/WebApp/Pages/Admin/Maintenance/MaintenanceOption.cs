namespace AirWeb.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;

    private MaintenanceOption() { }

    public static MaintenanceOption NotificationType { get; } =
        new() { SingularName = "Notification Type", PluralName = "Notification Types" };

    public static MaintenanceOption Office { get; } =
        new() { SingularName = "Office", PluralName = "Offices" };
}
