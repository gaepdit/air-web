using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.TestData;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalEntryTypeRepository()
    : NamedEntityRepository<EntryType>(EntryTypeData.GetData), IEntryTypeRepository;
