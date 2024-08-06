using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class NotificationTypeRepository(AppDbContext context)
    : NamedEntityRepository<NotificationType, AppDbContext>(context),
        INotificationTypeRepository;
