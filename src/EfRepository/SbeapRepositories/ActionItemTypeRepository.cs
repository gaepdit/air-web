using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.SbeapRepositories;

public sealed class ActionItemTypeRepository(AppDbContext context)
    : NamedEntityRepository<ActionItemType, AppDbContext>(context), IActionItemTypeRepository;
