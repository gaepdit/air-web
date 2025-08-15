using AirWeb.WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace AirWeb.WebApp.Platform.PageModelHelpers;

public static class TempDataExtensions
{
    private const string DisplayMessages = nameof(DisplayMessages);

    private static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class =>
        tempData[key] = JsonSerializer.Serialize(value);

    private static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        tempData.TryGetValue(key, out var o);
        return o is null ? null : JsonSerializer.Deserialize<T>((string)o);
    }

    public static void AddDisplayMessage(this ITempDataDictionary tempData, DisplayMessage.AlertContext context,
        string message)
    {
        var collection = GetDisplayMessages(tempData) ?? [];
        collection.Add(new DisplayMessage(context, message));
        tempData.Set(DisplayMessages, collection.ToArray());
    }

    public static ICollection<DisplayMessage>? GetDisplayMessages(this ITempDataDictionary tempData) =>
        tempData.Get<ICollection<DisplayMessage>>(DisplayMessages);
}
