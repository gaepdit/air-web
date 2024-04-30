using MyApp.AppServices.DtoBase;

namespace MyApp.AppServices.EntryTypes;

public record EntryTypeViewDto(Guid Id, string Name, bool Active) : StandardNamedEntityViewDto(Id, Name, Active);

public record EntryTypeCreateDto(string Name) : StandardNamedEntityCreateDto(Name);

public record EntryTypeUpdateDto(string Name, bool Active) : StandardNamedEntityUpdateDto(Name, Active);
