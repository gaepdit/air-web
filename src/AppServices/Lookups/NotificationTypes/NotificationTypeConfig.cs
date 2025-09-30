using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Lookups.LookupsBase;
using AirWeb.Domain.Lookups.NotificationTypes;
using AutoMapper;

namespace AirWeb.AppServices.Lookups.NotificationTypes;

// DTOs

public record NotificationTypeViewDto : LookupViewDto;

public record NotificationTypeCreateDto : LookupCreateDto;

public record NotificationTypeUpdateDto : LookupUpdateDto;

// App Services

public interface INotificationTypeService : ILookupService<NotificationTypeViewDto, NotificationTypeUpdateDto>;

public class NotificationTypeService(
    IMapper mapper,
    INotificationTypeRepository repository,
    INotificationTypeManager manager,
    IUserService userService)
    : LookupService<NotificationType, NotificationTypeViewDto, NotificationTypeUpdateDto>
        (mapper, repository, manager, userService), INotificationTypeService;

// Validators

public class NotificationTypeCreateValidator(INotificationTypeRepository repository)
    : LookupCreateValidator<NotificationTypeCreateDto, INotificationTypeRepository, NotificationType>(repository);

public class NotificationTypeUpdateValidator(INotificationTypeRepository repository)
    : LookupUpdateValidator<NotificationTypeUpdateDto, INotificationTypeRepository, NotificationType>
        (repository);
