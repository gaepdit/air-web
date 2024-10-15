namespace AirWeb.WebApp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Challenge();

        return Page();
    }
}
