using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public abstract class IndexBase : PageModel
{
    public IReadOnlyList<NamedEntityViewDto> Items { get; private set; } = default!;
    public MaintenanceOption ThisOption { get; private set; } = default!;
    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    protected async Task DoGet<TViewDto, TUpdateDto>(
        INamedEntityService<TViewDto, TUpdateDto> service,
        IAuthorizationService authorization,
        MaintenanceOption thisOption)
        where TViewDto : NamedEntityViewDto
    {
        ThisOption = thisOption;
        Items = await service.GetListAsync();
        IsSiteMaintainer = await authorization.Succeeded(User, Policies.SiteMaintainer);
    }
}
