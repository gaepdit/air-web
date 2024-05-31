using AutoMapper;
using AirWeb.AppServices.ServiceBase;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.EntryTypes;

namespace AirWeb.AppServices.EntryTypes;

public sealed class EntryTypeService(
    IMapper mapper,
    IEntryTypeRepository repository,
    IEntryTypeManager manager,
    IUserService userService)
    : MaintenanceItemService<EntryType, EntryTypeViewDto, EntryTypeUpdateDto>
        (mapper, repository, manager, userService),
        IEntryTypeService;
