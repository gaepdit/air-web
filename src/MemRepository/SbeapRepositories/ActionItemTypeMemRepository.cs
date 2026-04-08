using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AirWeb.TestData.Sbeap;

namespace AirWeb.MemRepository.SbeapRepositories;

public sealed class ActionItemTypeMemRepository()
    : NamedEntityRepository<ActionItemType>(ActionItemTypeData.GetActionItemTypes), IActionItemTypeRepository;
