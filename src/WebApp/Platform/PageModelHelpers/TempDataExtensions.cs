using AirWeb.WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace AirWeb.WebApp.Platform.PageModelHelpers;

public static class TempDataExtensions
{
    private const string DisplayMessages = nameof(DisplayMessages);

    extension(ITempDataDictionary tempData)
    {
        private void Set<T>(string key, T value) where T : class =>
            tempData[key] = JsonSerializer.Serialize(value);

        private T? Get<T>(string key) where T : class
        {
            tempData.TryGetValue(key, out var o);
            return o is null ? null : JsonSerializer.Deserialize<T>((string)o);
        }

        public void AddDisplayMessage(DisplayMessage.AlertContext context,
            string message)
        {
            var collection = tempData.GetDisplayMessages() ?? [];
            collection.Add(new DisplayMessage(context, message));
            tempData.Set(DisplayMessages, collection.ToArray());
        }

        public ICollection<DisplayMessage>? GetDisplayMessages() =>
            tempData.Get<ICollection<DisplayMessage>>(DisplayMessages);
    }
}
