using AirWeb.AppServices.Lookups.LookupsBase;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.Lookups.Offices;

public interface IOfficeService : ILookupService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id = null, bool includeInactive = false,
        CancellationToken token = default);
}
