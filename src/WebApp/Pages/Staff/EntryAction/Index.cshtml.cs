using AirWeb.AppServices.Permissions;

namespace AirWeb.WebApp.Pages.Staff.EntryAction;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class EntryActionIndexModel : PageModel
{
    public RedirectToPageResult OnGet() => RedirectToPage("../Index");
}
