using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AirWeb.Domain.Entities.WorkEntries;

public class Notification : BaseWorkEntry
{
    internal Notification(int? id) : base(id) => WorkEntryType = WorkEntryType.Notification;

    public NotificationType NotificationType { get; init; }
    public DateOnly ReceivedDate { get; init; }
    public DateOnly? DueDate { get; init; }
    public DateOnly? SentDate { get; init; }
    public bool FollowupTaken { get; init; }
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

#pragma warning disable S1133 : FUTURE: remove after data migration is validated
    [Obsolete("Permit Revocation was moved to separate entity")]
#pragma warning restore S1133
    [Description("Permit Revocation")]
    PermitRevocation = 3,
}
