using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Notification : BaseWorkEntry
{
    internal Notification(int? id) : base(id) => WorkEntryType = WorkEntryType.Notification;

    [StringLength(14)]
    public NotificationType NotificationType { get; set; }

    public DateOnly ReceivedDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public DateOnly? SentDate { get; set; }
    public bool FollowupTaken { get; set; }
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

    // [Obsolete("Permit Revocation was moved to separate entity")]
    // [Description("Permit Revocation")]
    // PermitRevocation = 3,
}
