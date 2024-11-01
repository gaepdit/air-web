namespace AirWeb.WebApp.Pages.Admin;

public class IndexModel : PageModel
{
    public IActionResult OnGet() => RedirectToPage("../Index");
}
