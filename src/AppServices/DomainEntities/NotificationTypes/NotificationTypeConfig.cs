using AirWeb.AppServices.DomainEntities.NamedEntitiesBase;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.NotificationTypes;
using AutoMapper;

namespace AirWeb.AppServices.DomainEntities.NotificationTypes;

// DTOs

public record NotificationTypeViewDto(Guid Id, string Name, bool Active) : NamedEntityViewDto(Id, Name, Active);

public record NotificationTypeCreateDto(string Name) : NamedEntityCreateDto(Name);

public record NotificationTypeUpdateDto(string Name, bool Active) : NamedEntityUpdateDto(Name, Active);

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
    : NamedEntityCreateValidator<NotificationTypeCreateDto, INotificationTypeRepository, NotificationType>
        (repository);

public class NotificationTypeUpdateValidator(INotificationTypeRepository repository)
    : NamedEntityUpdateValidator<NotificationTypeUpdateDto, INotificationTypeRepository, NotificationType>
        (repository);
