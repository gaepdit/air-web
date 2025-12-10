using AirWeb.AppServices.Lookups.LookupsBase;
using AirWeb.Domain.Lookups.Offices;

namespace AirWeb.AppServices.Lookups.Offices;

// DTOs

public record OfficeViewDto : LookupViewDto;

public record OfficeCreateDto : LookupCreateDto;

public record OfficeUpdateDto : LookupUpdateDto;

// Validators

public class OfficeCreateValidator(IOfficeRepository repository)
    : LookupCreateValidator<OfficeCreateDto, IOfficeRepository, Office>(repository);

public class OfficeUpdateValidator(IOfficeRepository repository)
    : LookupUpdateValidator<OfficeUpdateDto, IOfficeRepository, Office>(repository);
