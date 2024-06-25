using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.DomainEntities.MaintenanceItemsBase;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.NotificationTypes;
using AutoMapper;

namespace AirWeb.AppServices.DomainEntities.NotificationTypes;

// DTOs

public record NotificationTypeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record NotificationTypeCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record NotificationTypeUpdateDto(string Name, bool Active) : StandardNamedEntityUpdateDto(Name, Active);

// App Service

public interface INotificationTypeService : IMaintenanceItemService<NotificationTypeViewDto, NotificationTypeUpdateDto>;

public class NotificationTypeService(
    IMapper mapper,
    INotificationTypeRepository repository,
    INotificationTypeManager manager,
    IUserService userService)
    : MaintenanceItemService<NotificationType, NotificationTypeViewDto, NotificationTypeUpdateDto>
        (mapper, repository, manager, userService), INotificationTypeService;
