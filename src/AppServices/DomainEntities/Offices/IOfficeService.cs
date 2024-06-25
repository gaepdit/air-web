using AirWeb.AppServices.DomainEntities.MaintenanceItemsBase;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.DomainEntities.Offices;

public interface IOfficeService : IMaintenanceItemService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);
}
