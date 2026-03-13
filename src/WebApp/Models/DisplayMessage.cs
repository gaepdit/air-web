using System.Text.Json.Serialization;

namespace AirWeb.WebApp.Models;

public record DisplayMessage(DisplayMessage.AlertContext Context, string Message)
{
    public enum AlertContext
    {
        Success,
        Danger,
        Warning,
        Info,
    }

    [JsonIgnore]
    public string AlertClass => Context switch
    {
        AlertContext.Success => "alert-success",
        AlertContext.Danger => "alert-danger",
        AlertContext.Warning => "alert-warning",
        AlertContext.Info => "alert-info",
        _ => string.Empty,
    };
}
