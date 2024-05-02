using GaEpd.AppLibrary.ListItems;
using AirWeb.AppServices.ServiceBase;

namespace AirWeb.AppServices.Offices;

public interface IOfficeService : IMaintenanceItemService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);
}
