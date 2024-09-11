using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using AirWeb.AppServices.Users;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AutoMapper;

namespace AirWeb.AppServices.NamedEntities.NotificationTypes;

// DTOs

public record NotificationTypeViewDto : NamedEntityViewDto;

public record NotificationTypeCreateDto : NamedEntityCreateDto;

public record NotificationTypeUpdateDto : NamedEntityUpdateDto;

// App Services

public interface INotificationTypeService : INamedEntityService<NotificationTypeViewDto, NotificationTypeUpdateDto>;

public class NotificationTypeService(
    IMapper mapper,
    INotificationTypeRepository repository,
    INotificationTypeManager manager,
    IUserService userService)
    : NamedEntityService<NotificationType, NotificationTypeViewDto, NotificationTypeUpdateDto>
        (mapper, repository, manager, userService), INotificationTypeService;

// Validators

public class NotificationTypeCreateValidator(INotificationTypeRepository repository)
    : NamedEntityCreateValidator<NotificationTypeCreateDto, INotificationTypeRepository, NotificationType>(repository);

public class NotificationTypeUpdateValidator(INotificationTypeRepository repository)
    : NamedEntityUpdateValidator<NotificationTypeUpdateDto, INotificationTypeRepository, NotificationType>
        (repository);
