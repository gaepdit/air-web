using System.Reflection;

namespace PermitSearch.Platform;

internal static class AppSettings
{
    public static string BaseUrl { get; private set; } = string.Empty;
    public static bool IsDevelopment { get; private set; }

    public static void LoadSettings(this IHostApplicationBuilder builder)
    {
        BaseUrl = builder.Configuration.GetValue<string>("BaseUrl") + "/Permits";
        IsDevelopment = builder.Environment.IsDevelopment();
    }

    public static string GetVersion()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var segments = (entryAssembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? entryAssembly?.GetName().Version?.ToString() ?? "").Split('+');
        return segments[0] + (segments.Length > 0 ? $"+{segments[1][..Math.Min(7, segments[1].Length)]}" : "");
    }
}
