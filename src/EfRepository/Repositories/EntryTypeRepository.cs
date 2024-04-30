using MyApp.Domain.Entities.EntryTypes;
using MyApp.EfRepository.DbContext;

namespace MyApp.EfRepository.Repositories;

public sealed class EntryTypeRepository(AppDbContext dbContext) :
    NamedEntityRepository<EntryType, AppDbContext>(dbContext), IEntryTypeRepository;
