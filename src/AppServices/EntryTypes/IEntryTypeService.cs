using MyApp.AppServices.ServiceBase;

namespace MyApp.AppServices.EntryTypes;

public interface IEntryTypeService : IMaintenanceItemService<EntryTypeViewDto, EntryTypeUpdateDto>;
