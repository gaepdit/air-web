using GaEpd.AppLibrary.Domain.Entities;
using AirWeb.Domain;
using System.ComponentModel.DataAnnotations;

namespace AirWeb.AppServices.DtoBase;

public abstract record StandardNamedEntityViewDto(Guid Id, string Name, bool Active) : INamedEntity;

public abstract record StandardNamedEntityCreateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    string Name
);

public abstract record StandardNamedEntityUpdateDto
(
    [Required(AllowEmptyStrings = false)]
    [StringLength(AppConstants.MaximumNameLength,
        MinimumLength = AppConstants.MinimumNameLength)]
    string Name,
    bool Active
);
