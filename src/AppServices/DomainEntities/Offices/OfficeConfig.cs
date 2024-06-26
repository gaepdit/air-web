using AirWeb.AppServices.DomainEntities.NamedEntitiesBase;
using AirWeb.Domain.Entities.Offices;

namespace AirWeb.AppServices.DomainEntities.Offices;

// DTOs

public record OfficeViewDto(Guid Id, string Name, bool Active) : NamedEntityViewDto(Id, Name, Active);

public record OfficeCreateDto(string Name) : NamedEntityCreateDto(Name);

public record OfficeUpdateDto(string Name, bool Active) : NamedEntityUpdateDto(Name, Active);

// Validators

public class OfficeCreateValidator(IOfficeRepository repository)
    : NamedEntityCreateValidator<OfficeCreateDto, IOfficeRepository, Office>(repository);

public class OfficeUpdateValidator(IOfficeRepository repository)
    : NamedEntityUpdateValidator<OfficeUpdateDto, IOfficeRepository, Office>(repository);
