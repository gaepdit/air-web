using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.AppServices.Core.EntityServices.LookupsBase;
using AirWeb.Core.AppRoles;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

public abstract class IndexBase : PageModel
{
    public IReadOnlyList<LookupViewDto> Items { get; private set; } = null!;
    public bool IsMaintainer { get; private set; }
    public virtual MaintenanceOption ThisOption => null!;
    public virtual AuthorizationPolicy Policy => null!;
    public virtual AppRole AppRole => null!;

    [TempData]
    public Guid? HighlightId { get; set; }

    protected async Task DoGet<TViewDto, TUpdateDto>(
        ILookupService<TViewDto, TUpdateDto> service,
        IAuthorizationService authorization)
        where TViewDto : LookupViewDto
    {
        Items = await service.GetListAsync();
        IsMaintainer = await authorization.Succeeded(User, Policy);
    }
}
