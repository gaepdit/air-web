using IaipDataService.Facilities;

namespace AirWeb.WebApp.Pages.Permits;

public class View : PageModel
{
    public async Task<IActionResult> OnGetAsync([FromServices] IFacilityService service, [FromRoute] string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return RedirectToPage("Index");

        var document = await service.GetPermitFileAsync(fileName);
        if (document is null || document.Length == 0)
        {
            var page = Page();
            page.StatusCode = StatusCodes.Status404NotFound;
            return page;
        }

        Response.Headers.Append("Content-Disposition", $"inline;filename={fileName}.pdf");
        return File(document, contentType: "application/pdf");
    }
}
