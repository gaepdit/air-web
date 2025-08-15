using AirWeb.WebApp.Platform.PrintoutModels;
using JetBrains.Annotations;

namespace AirWeb.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string? Version { get; private set; }
    public static Support Support { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static OrganizationInfo OrganizationInfo { get; } = new();
    public static string? OrgNotificationsApiUrl { get; private set; }

    public record Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }
}

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record Support
{
    public string? CustomerSupportEmail { get; init; }
    public string? TechnicalSupportEmail { get; init; }
    public string? TechnicalSupportSite { get; init; }
    public Uri? TechnicalSupportSiteUrl => TechnicalSupportSite is null ? null : new Uri(TechnicalSupportSite);
}
