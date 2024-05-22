using AirWeb.AppServices.Notifications;
using AirWeb.Domain.Entities.WorkEntries;

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
    /// If the <see cref="WorkEntry"/> is successfully created, contains the ID of the new WorkEntry. 
    /// </summary>
    /// <value>The WorkEntry ID if the operation succeeded, otherwise null.</value>
    public TKey? Id { get; }

    public NotificationResult? NotificationResult { get; set; }
}
