using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.EfRepository.DbContext;

namespace AirWeb.EfRepository.Repositories;

public sealed class EntryTypeRepository(AppDbContext dbContext) :
    NamedEntityRepository<EntryType, AppDbContext>(dbContext), IEntryTypeRepository;
