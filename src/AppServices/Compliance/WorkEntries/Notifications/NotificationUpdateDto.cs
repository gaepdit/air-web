namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public record NotificationUpdateDto : NotificationCommandDto
{
    public bool IsClosed { get; init; }
    public bool IsDeleted { get; init; }
}
