using MyApp.AppServices.Offices;
using MyApp.AppServices.Permissions;
using MyApp.AppServices.Permissions.Helpers;

namespace MyApp.WebApp.Api;

[ApiController]
[Route("api/offices")]
[Produces("application/json")]
public class OfficeApiController(
    IOfficeService officeService,
    IAuthorizationService authorization) : Controller
{
    [HttpGet]
    public async Task<IReadOnlyList<OfficeViewDto>> ListOfficesAsync() =>
        await officeService.GetListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeViewDto>> GetOfficeAsync([FromRoute] Guid id)
    {
        var item = await officeService.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }

    [HttpGet("{id:guid}/staff")]
    public async Task<JsonResult> GetStaffAsync([FromRoute] Guid id) =>
        Json(await officeService.GetStaffAsListItemsAsync(id));

    [HttpGet("{id:guid}/all-staff")]
    public async Task<IActionResult> GetAllStaffAsync([FromRoute] Guid id) =>
        await authorization.Succeeded(User, Policies.ActiveUser)
            ? Json(await officeService.GetStaffAsListItemsAsync(id, includeInactive: true))
            : Unauthorized();
}
