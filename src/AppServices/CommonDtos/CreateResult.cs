using AirWeb.AppServices.Notifications;

namespace AirWeb.AppServices.CommonDtos;

// Used for creating entities
public record CreateResult<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Returns <see cref="CreateResult{TKey}"/> indicating a successfully created entity.
    /// </summary>
    /// <param name="id">The ID of the new entity.</param>
    /// <returns><see cref="CreateResult{TKey}"/> indicating a successful operation with a NotificationResult
    /// indicating the status of any notifications sent.</returns>
    public CreateResult(TKey id) => Id = id;

    /// <summary>
    /// If the entity is successfully created, contains its ID. 
    /// </summary>
    /// <value>The ID of the entity.</value>
    public TKey Id { get; }

    /// <summary>
    /// Contains the <see cref="NotificationResult"/> generated from an attempted notification.
    /// </summary>
    public NotificationResult? NotificationResult { get; set; }
}
