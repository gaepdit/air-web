using AirWeb.Domain.Entities.EntryActions;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class EntryActionRepository(AppDbContext dbContext)
    : BaseRepository<EntryAction, AppDbContext>(dbContext), IEntryActionRepository;
