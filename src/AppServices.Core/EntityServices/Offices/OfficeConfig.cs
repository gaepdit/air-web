using AirWeb.AppServices.Core.EntityServices.LookupsBase;
using AirWeb.Domain.Core.Entities;

namespace AirWeb.AppServices.Core.EntityServices.Offices;

// DTOs

public record OfficeViewDto : LookupViewDto;

public record OfficeCreateDto : LookupCreateDto;

public record OfficeUpdateDto : LookupUpdateDto;

// Validators

public class OfficeCreateValidator(IOfficeRepository repository)
    : LookupCreateValidator<OfficeCreateDto, IOfficeRepository, Office>(repository);

public class OfficeUpdateValidator(IOfficeRepository repository)
    : LookupUpdateValidator<OfficeUpdateDto, IOfficeRepository, Office>(repository);
