using AirWeb.AppServices.Core.EntityServices.LookupsBase;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;

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
    IUserService userService,
    IMemoryCache cache,
    ILogger<NotificationTypeService> logger)
    : LookupService<NotificationType, NotificationTypeViewDto, NotificationTypeUpdateDto>
        (mapper, repository, manager, userService, cache, logger), INotificationTypeService;

// Validators

public class NotificationTypeCreateValidator(INotificationTypeRepository repository)
    : LookupCreateValidator<NotificationTypeCreateDto, INotificationTypeRepository, NotificationType>(repository);

public class NotificationTypeUpdateValidator(INotificationTypeRepository repository)
    : LookupUpdateValidator<NotificationTypeUpdateDto, INotificationTypeRepository, NotificationType>
        (repository);
