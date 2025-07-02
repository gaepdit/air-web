using AirWeb.WebApp.Platform.Settings;
using Mindscape.Raygun4Net.AspNetCore;
using Mindscape.Raygun4Net.Extensions.Logging;

namespace AirWeb.WebApp.Platform.AppConfiguration;

internal static class ErrorLogging
{
    public static void ConfigureErrorLogging(this WebApplicationBuilder builder)
    {
        if (string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) return;

        builder.Services.AddRaygun(options =>
        {
            options.ApiKey = AppSettings.RaygunSettings.ApiKey;
            options.ApplicationVersion = AppSettings.Version;
            options.IgnoreFormFieldNames = ["*Password"];
            options.EnvironmentVariables.Add("ASPNETCORE_*");
        });
        builder.Services.AddRaygunUserProvider();
        builder.Logging.AddRaygunLogger(options =>
        {
            options.MinimumLogLevel = LogLevel.Warning;
            options.OnlyLogExceptions = false;
        });
    }
}
