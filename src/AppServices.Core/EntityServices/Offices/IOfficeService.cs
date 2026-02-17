using AirWeb.AppServices.Core.EntityServices.LookupsBase;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.Core.EntityServices.Offices;

public interface IOfficeService : ILookupService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id = null, bool includeInactive = false,
        CancellationToken token = default);
}
