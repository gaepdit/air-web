using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.EfRepository.Contexts;

namespace AirWeb.EfRepository.SbeapRepositories;

public sealed class ActionItemRepository(AppDbContext context)
    : BaseRepository<ActionItem, Guid, AppDbContext>(context), IActionItemRepository;
