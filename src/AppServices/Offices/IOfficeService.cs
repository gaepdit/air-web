using GaEpd.AppLibrary.ListItems;
using MyApp.AppServices.ServiceBase;

namespace MyApp.AppServices.Offices;

public interface IOfficeService : IMaintenanceItemService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);
}
