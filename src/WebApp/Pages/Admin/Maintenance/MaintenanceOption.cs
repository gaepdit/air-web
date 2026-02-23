namespace AirWeb.WebApp.Pages.Admin.Maintenance;

public record MaintenanceOption
{
    public string SingularName { get; private init; }
    public string PluralName { get; private init; }
    public bool StartsWithVowelSound { get; private init; }

    private MaintenanceOption(string singularName, string pluralName, bool startsWithVowelSound = false)
    {
        SingularName = singularName;
        PluralName = pluralName;
        StartsWithVowelSound = startsWithVowelSound;
    }

    // Common
    public static MaintenanceOption Office =>
        new(singularName: "Office", pluralName: "Offices", startsWithVowelSound: true);

    // Compliance
    public static MaintenanceOption NotificationType =>
        new(singularName: "Compliance Notification Type", pluralName: "Compliance Notification Types",
            startsWithVowelSound: false);

    // SBEAP
    public static MaintenanceOption ActionItemType =>
        new(singularName: "SBEAP Action Item Type", pluralName: "SBEAP Action Item Types", startsWithVowelSound: true);

    public static MaintenanceOption Agency =>
        new(singularName: "SBEAP Agency", pluralName: "SBEAP Agencies", startsWithVowelSound: true);
}
