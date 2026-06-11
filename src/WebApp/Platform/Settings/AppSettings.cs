using AirWeb.WebApp.Pages.Print.PrintSectionModels;
using JetBrains.Annotations;

namespace AirWeb.WebApp.Platform.Settings;

internal static partial class AppSettings
{
    public static string? Version { get; private set; }
    public static string? SimpleVersion => Version?.Split('+')[0];

    internal static string Env { get; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown";
    public static string ShortEnv => Env switch { "Production" => "prod", "Staging" => "uat", _ => "dev" };

    public static Support Support { get; } = new();
    public static Raygun RaygunSettings { get; } = new();
    public static DataDog DataDogSettings { get; } = new();
    public static OrganizationInfo OrganizationInfo { get; } = new();
    public static string? OrgNotificationsApiUrl { get; private set; }

    public record Raygun
    {
        public string? ApiKey { get; [UsedImplicitly] init; }
    }

    public record DataDog
    {
        public string? ClientToken { get; [UsedImplicitly] init; }
        public string? ApplicationId { get; [UsedImplicitly] init; }
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
