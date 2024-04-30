using MyApp.Domain.Entities.EntryActions;
using MyApp.TestData;

namespace MyApp.LocalRepository.Repositories;

public sealed class LocalEntryActionRepository() 
    : BaseRepository<EntryAction, Guid>(EntryActionData.GetData), IEntryActionRepository;
