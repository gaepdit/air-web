using AirWeb.AppServices.Core.AuthorizationServices;

namespace AirWeb.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class RolesModel : PageModel;
