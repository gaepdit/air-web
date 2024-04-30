using MyApp.Domain.Entities.EntryActions;
using MyApp.EfRepository.DbContext;

namespace MyApp.EfRepository.Repositories;

public sealed class EntryActionRepository(AppDbContext dbContext)
    : BaseRepository<EntryAction, AppDbContext>(dbContext), IEntryActionRepository;
