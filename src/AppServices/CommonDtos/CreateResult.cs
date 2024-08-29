using AirWeb.AppServices.AppNotifications;

namespace AirWeb.AppServices.CommonDtos;

// Used for creating entities
public record CreateResult<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Returns <see cref="CreateResult{TKey}"/> indicating a successfully created entity.
    /// </summary>
    /// <param name="id">The ID of the new entity.</param>
    /// <returns><see cref="CreateResult{TKey}"/> indicating a successful operation with no <see cref="AppNotificationResult"/>.</returns>
    public CreateResult(TKey id) => Id = id;

    /// <summary>
    /// Returns <see cref="CreateResult{TKey}"/> indicating a successfully created entity.
    /// </summary>
    /// <param name="id">The ID of the new entity.</param>
    /// <param name="appNotificationResult">The <see cref="AppNotificationResult"/> generated from an attempted
    /// notification.</param>
    /// <returns><see cref="CreateResult{TKey}"/> indicating a successful operation with a <see cref="AppNotificationResult"/>
    /// indicating the status of any notifications sent.</returns>
    public CreateResult(TKey id, AppNotificationResult appNotificationResult)
    {
        Id = id;
        AppNotificationResult = appNotificationResult;
    }

    /// <summary>
    /// If the entity is successfully created, contains its ID. 
    /// </summary>
    /// <value>The ID of the entity.</value>
    public TKey Id { get; }

    /// <summary>
    /// Contains the <see cref="AppNotificationResult"/> generated from an attempted app notification.
    /// </summary>
    public AppNotificationResult? AppNotificationResult { get; }

    public bool HasAppNotificationFailure => !string.IsNullOrEmpty(AppNotificationResult?.FailureMessage);
}
