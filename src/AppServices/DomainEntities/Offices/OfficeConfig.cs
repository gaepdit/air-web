using AirWeb.AppServices.DomainEntities.NamedEntitiesBase;
using AirWeb.Domain.Entities.Offices;

namespace AirWeb.AppServices.DomainEntities.Offices;

// DTOs

public record OfficeViewDto : NamedEntityViewDto;

public record OfficeCreateDto : NamedEntityCreateDto;

public record OfficeUpdateDto : NamedEntityUpdateDto;

// Validators

public class OfficeCreateValidator(IOfficeRepository repository)
    : NamedEntityCreateValidator<OfficeCreateDto, IOfficeRepository, Office>(repository);

public class OfficeUpdateValidator(IOfficeRepository repository)
    : NamedEntityUpdateValidator<OfficeUpdateDto, IOfficeRepository, Office>(repository);
