using AirWeb.Domain;
using GaEpd.AppLibrary.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.DomainEntities.NamedEntitiesBase;

public record NamedEntityViewDto : INamedEntity
{
    protected NamedEntityViewDto(Guid Id, string Name, bool Active)
    {
        this.Id = Id;
        this.Name = Name;
        this.Active = Active;
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public bool Active { get; init; }
}

public record NamedEntityCreateDto
{
    protected NamedEntityCreateDto(string Name) => this.Name = Name;

    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    public string Name { get; init; }
}

public record NamedEntityUpdateDto
{
    protected NamedEntityUpdateDto(string Name, bool Active)
    {
        this.Name = Name;
        this.Active = Active;
    }

    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    public string Name { get; init; }

    public bool Active { get; init; }
}
