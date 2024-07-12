using AirWeb.AppServices.DomainEntities.Offices;
using AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.Offices;

public class OfficeIndexModel : IndexBase
{
    public async Task OnGetAsync(
        [FromServices] IOfficeService service,
        [FromServices] IAuthorizationService authorization) =>
        await DoGet(service, authorization, MaintenanceOption.Office);
}
