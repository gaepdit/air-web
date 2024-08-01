using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class NotificationTypeRepository(AppDbContext dbContext)
    : NamedEntityRepository<NotificationType, AppDbContext>(dbContext),
        INotificationTypeRepository;
