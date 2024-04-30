using AutoMapper;
using MyApp.AppServices.ServiceBase;
using MyApp.AppServices.UserServices;
using MyApp.Domain.Entities.EntryTypes;

namespace MyApp.AppServices.EntryTypes;

public sealed class EntryTypeService(
    IMapper mapper,
    IEntryTypeRepository repository,
    IEntryTypeManager manager,
    IUserService userService)
    : MaintenanceItemService<EntryType, EntryTypeViewDto, EntryTypeUpdateDto>
        (mapper, repository, manager, userService),
        IEntryTypeService;
