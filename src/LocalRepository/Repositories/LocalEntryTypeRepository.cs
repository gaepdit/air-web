using MyApp.Domain.Entities.EntryTypes;
using MyApp.TestData;

namespace MyApp.LocalRepository.Repositories;

public sealed class LocalEntryTypeRepository()
    : NamedEntityRepository<EntryType>(EntryTypeData.GetData), IEntryTypeRepository;
