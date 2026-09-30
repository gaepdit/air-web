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
}
