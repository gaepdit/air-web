using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.NamedEntities.Offices;

public interface IOfficeService : INamedEntityService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);
}
