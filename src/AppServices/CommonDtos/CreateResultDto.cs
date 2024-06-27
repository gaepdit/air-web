using AirWeb.AppServices.Notifications;

namespace AirWeb.AppServices.CommonDtos;

// Used for creating entities
public record CreateResultDto<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Returns <see cref="CreateResultDto{TKey}"/> indicating a successfully created entity.
    /// </summary>
    /// <param name="id">The ID of the new entity.</param>
    /// <returns><see cref="CreateResultDto{TKey}"/> indicating a successful operation with a NotificationResult
    /// indicating the status of any notifications sent.</returns>
    public CreateResultDto(TKey id) => Id = id;

    /// <summary>
    /// If the entity is successfully created, contains its ID. 
    /// </summary>
    /// <value>The ID of the entity if the operation succeeded, otherwise null.</value>
    public TKey? Id { get; }

    /// <summary>
    /// Contains the <see cref="NotificationResult"/> generated from an attempted notification.
    /// </summary>
    public NotificationResult? NotificationResult { get; set; }
}
