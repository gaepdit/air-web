using AirWeb.Domain;
using GaEpd.AppLibrary.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.NamedEntities.NamedEntitiesBase;

public abstract record NamedEntityViewDto : INamedEntity
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public bool Active { get; init; }
}

public abstract record NamedEntityCreateDto : INamedEntity
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    public required string Name { get; init; }
}

public abstract record NamedEntityUpdateDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    public required string Name { get; init; }

    public bool Active { get; init; }
}
