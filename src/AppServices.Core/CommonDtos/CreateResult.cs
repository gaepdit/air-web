namespace AirWeb.AppServices.Core.CommonDtos;

// Used for creating entities, returning the ID of the new entity. Can also include non-failure warning messages.
public record CreateResult<TKey> : CommandResult
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// If the entity is successfully created, contains its ID.
    /// </summary>
    /// <value>The ID of the entity.</value>
    public TKey? Id { get; private set; }

    // Static constructors
    public static CreateResult<TKey> Create(TKey id, string? warningMessage = null) =>
        new() { Id = id, WarningMessage = warningMessage };
}
