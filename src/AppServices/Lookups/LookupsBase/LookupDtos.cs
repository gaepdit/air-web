using AirWeb.Core;
using GaEpd.AppLibrary.Domain.Entities;

namespace AirWeb.AppServices.Lookups.LookupsBase;

public abstract record LookupViewDto : INamedEntity
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public bool Active { get; init; }
}

public abstract record LookupCreateDto : INamedEntity
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    public string Name { get; init; } = string.Empty;
}

public abstract record LookupUpdateDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    public string Name { get; init; } = null!;

    public bool Active { get; init; }
}
