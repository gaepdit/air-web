using AirWeb.WebApp.Platform.Settings;
using GaEpd.AppLibrary.Apis;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;

namespace AirWeb.WebApp.Platform.OrgNotifications;

// Organizational notifications

public static class OrgNotificationsServiceExtensions
{
    public static void AddOrgNotifications(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IOrgNotifications, OrgNotifications>();
    }
}

public interface IOrgNotifications
{
    Task<List<OrgNotification>> GetOrgNotificationsAsync();
}

public record OrgNotification
{
    public required string Message { get; [UsedImplicitly] init; }
}

public class OrgNotifications(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache,
    ILogger<OrgNotifications> logger) : IOrgNotifications
{
    private const string ApiEndpoint = "/current";
    private const string CacheKey = nameof(OrgNotifications);

    public async Task<List<OrgNotification>> GetOrgNotificationsAsync()
    {
        if (AppSettings.OrgNotificationsApiUrl is null) return [];

        if (cache.TryGetValue(CacheKey, out List<OrgNotification>? notifications) && notifications != null)
            return notifications;

        try
        {
            notifications = await httpClientFactory.FetchApiDataAsync<List<OrgNotification>>(
                AppSettings.OrgNotificationsApiUrl, ApiEndpoint, "NotificationsClient");
            if (notifications is null) return [];
            cache.Set(CacheKey, notifications, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
        }
        catch (Exception ex)
        {
            // If the API is unresponsive or other error occurs, no notifications will be displayed.
            logger.LogError(ex, "Failed to fetch organizational notifications.");
            return [];
        }

        return notifications;
    }
}