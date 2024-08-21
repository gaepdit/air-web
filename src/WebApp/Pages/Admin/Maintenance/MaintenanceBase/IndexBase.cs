using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.Domain.Identity;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public abstract class IndexBase : PageModel
{
    public IReadOnlyList<NamedEntityViewDto> Items { get; private set; } = default!;
    public bool IsMaintainer { get; private set; }
    public virtual MaintenanceOption ThisOption => null!;
    public virtual AuthorizationPolicy Policy => null!;
    public virtual AppRole AppRole => null!;

    [TempData]
    public Guid? HighlightId { get; set; }

    protected async Task DoGet<TViewDto, TUpdateDto>(
        INamedEntityService<TViewDto, TUpdateDto> service,
        IAuthorizationService authorization)
        where TViewDto : NamedEntityViewDto
    {
        Items = await service.GetListAsync();
        IsMaintainer = await authorization.Succeeded(User, Policy);
    }
}
