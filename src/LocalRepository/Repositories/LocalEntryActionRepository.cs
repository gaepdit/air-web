using AirWeb.Domain.Entities.EntryActions;
using AirWeb.TestData;

namespace AirWeb.LocalRepository.Repositories;

public sealed class LocalEntryActionRepository() 
    : BaseRepository<EntryAction, Guid>(EntryActionData.GetData), IEntryActionRepository;
