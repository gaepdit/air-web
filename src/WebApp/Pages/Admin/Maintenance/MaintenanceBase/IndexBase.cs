using AirWeb.AppServices.DomainEntities.NamedEntitiesBase;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public abstract class IndexBase : PageModel
{
    public IReadOnlyList<NamedEntityViewDto> Items { get; protected set; } = default!;
    public MaintenanceOption ThisOption { get; protected set; } = default!;
    public bool IsSiteMaintainer { get; protected set; }

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
