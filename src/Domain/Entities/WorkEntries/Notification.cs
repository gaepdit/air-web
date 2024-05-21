using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Notification : WorkEntry
{
    internal Notification(int? id) : base(id) => WorkEntryType = WorkEntryType.Notification;

    public NotificationType NotificationType { get; init; }
}

// Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationType
{
    Other = 1,
    Startup = 2,
    [Description("Response Letter")] ResponseLetter = 6,
    Malfunction = 7,
    Deviation = 8,

    [Obsolete("Permit Revocation was moved to separate entity")] [Description("Permit Revocation")]
    PermitRevocation = 3,
}
