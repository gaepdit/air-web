using AirWeb.WebApp.Platform.Settings;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class BindingsConfiguration
{
    public static void BindSettings(WebApplicationBuilder builder)
    {
        // Bind app settings.
        builder.Configuration.GetSection(nameof(AppSettings.SupportSettings))
            .Bind(AppSettings.SupportSettings);

        builder.Configuration.GetSection(nameof(AppSettings.OrganizationInfo))
            .Bind(AppSettings.OrganizationInfo);

        builder.Configuration.GetSection(nameof(AppSettings.RaygunSettings))
            .Bind(AppSettings.RaygunSettings);

        // Dev settings should only be used in the development environment and when explicitly enabled.
        var devConfig = builder.Configuration.GetSection(nameof(AppSettings.DevSettings));
        var useDevConfig = builder.Environment.IsDevelopment() && devConfig.Exists() &&
                           Convert.ToBoolean(devConfig[nameof(AppSettings.DevSettings.UseDevSettings)]);

        if (useDevConfig)
            devConfig.Bind(AppSettings.DevSettings);
        else
            AppSettings.DevSettings = AppSettings.ProductionDefault;
    }
}
