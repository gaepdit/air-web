using AirWeb.Domain.Sbeap.Entities.ActionItems;
using AirWeb.TestData.Sbeap;

namespace AirWeb.MemRepository.SbeapRepositories;

public sealed class ActionMemItemRepository()
    : BaseRepository<ActionItem, Guid>(ActionItemData.GetActionItems), IActionItemRepository;
