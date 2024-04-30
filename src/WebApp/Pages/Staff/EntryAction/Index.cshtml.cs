using MyApp.AppServices.Permissions;

namespace MyApp.WebApp.Pages.Staff.EntryAction;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class EntryActionIndexModel : PageModel
{
    public RedirectToPageResult OnGet() => RedirectToPage("../Index");
}
