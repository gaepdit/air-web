using AirWeb.AppServices.DomainEntities.NamedEntitiesBase;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.AppServices.DomainEntities.Offices;

public interface IOfficeService : INamedEntityService<OfficeViewDto, OfficeUpdateDto>
{
    Task<IReadOnlyList<ListItem<string>>> GetStaffAsListItemsAsync(Guid? id, bool includeInactive = false,
        CancellationToken token = default);
}
