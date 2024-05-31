namespace AirWeb.Domain.Entities.EntryTypes;

public class EntryTypeManager(IEntryTypeRepository repository)
    : NamedEntityManager<EntryType, IEntryTypeRepository>(repository), IEntryTypeManager;
