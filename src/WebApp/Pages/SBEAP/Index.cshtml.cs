namespace AirWeb.WebApp.Pages.SBEAP;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("../Index");
}
