using AutoMapper;
using AirWeb.AppServices.ServiceBase;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.NotificationTypes;

namespace AirWeb.AppServices.EntryTypes;

public sealed class EntryTypeService(
    IMapper mapper,
    INotificationTypeRepository repository,
    INotificationTypeManager manager,
    IUserService userService)
    : MaintenanceItemService<NotificationType, EntryTypeViewDto, EntryTypeUpdateDto>
        (mapper, repository, manager, userService),
        IEntryTypeService;
