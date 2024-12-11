using AirWeb.WebApp.Platform.PrintoutModels;
using JetBrains.Annotations;

namespace AirWeb.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    // Organization settings
    public static OrganizationInfo OrganizationInfo { get; set; } = new();
    public static SupportSettingsSection SupportSettings { get; } = new();

    public record SupportSettingsSection
    {
        public string? CustomerSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportSite { get; [UsedImplicitly] init; }
        public string? InformationalVersion { get; set; }
        public string? InformationalBuild { get; set; }
    }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public record RaygunClientSettings
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
        public bool ExcludeErrorsFromLocal { get; [UsedImplicitly] init; }
    }
}
