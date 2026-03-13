using AirWeb.AppServices.Core.EntityServices.LookupsBase;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AutoMapper;

namespace AirWeb.AppServices.Sbeap.Agencies;

// DTOs

public record AgencyViewDto : LookupViewDto;

public record AgencyCreateDto : LookupCreateDto;

public record AgencyUpdateDto : LookupUpdateDto;

// App Services

public interface IAgencyService : ILookupService<AgencyViewDto, AgencyUpdateDto>;

public class AgencyService(
    IAgencyRepository repository,
    IAgencyManager manager,
    IMapper mapper,
    IUserService userService)
    : LookupService<Agency, AgencyViewDto, AgencyUpdateDto>
        (mapper, repository, manager, userService), IAgencyService;

// Validators

public class AgencyCreateValidator(IAgencyRepository repository)
    : LookupCreateValidator<AgencyCreateDto, IAgencyRepository, Agency>(repository);

public class AgencyUpdateValidator(IAgencyRepository repository)
    : LookupUpdateValidator<AgencyUpdateDto, IAgencyRepository, Agency>
        (repository);
