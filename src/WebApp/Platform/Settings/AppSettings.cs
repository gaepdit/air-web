using AirWeb.WebApp.Platform.PrintoutModels;
using JetBrains.Annotations;
using System.Reflection;

namespace AirWeb.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string Version { get; } = GetVersion();

    // Organization settings
    public static OrganizationInfo OrganizationInfo { get; set; } = new();

    // Support settings
    public static SupportSettingsSection SupportSettings { get; } = new();

    public record SupportSettingsSection
    {
        public string? CustomerSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportEmail { get; [UsedImplicitly] init; }
        public string? TechnicalSupportSite { get; [UsedImplicitly] init; }
    }

    // Raygun client settings
    public static RaygunClientSettings RaygunSettings { get; } = new();

    public record RaygunClientSettings
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
        public bool ExcludeErrorsFromLocal { get; [UsedImplicitly] init; }
    }

    private static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
