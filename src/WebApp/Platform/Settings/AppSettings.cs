using AirWeb.WebApp.Platform.PrintoutModels;
using JetBrains.Annotations;

namespace AirWeb.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string? Version { get; set; }
    public static Support SupportSettings { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static OrganizationInfo OrganizationInfo { get; } = new();

    // Organizational notifications
    public static string? OrgNotificationsApiUrl { get; set; }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public record Support
    {
        public string? CustomerSupportEmail { get; init; }
        public string? TechnicalSupportEmail { get; init; }
        public string? TechnicalSupportSite { get; init; }
    }

    public record Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }
}
