using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PermitSearch.Platform;
using System.Diagnostics.CodeAnalysis;
using ZLogger;
using UrlRegex = PermitSearch.Platform.UrlRegex;

namespace PermitSearch.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    public IActionResult OnGet([FromQuery] string? id, [FromQuery] string? airsNumber)
    {
        var pathAndQuery = Request.GetEncodedPathAndQuery();

        if (id != null && UrlRegex.IsValidPermit(id))
            return RedirectMaybePermanent(pathAndQuery, destination: $"/View/{id}");

        if (airsNumber != null && UrlRegex.IsValidAirs(airsNumber))
            return RedirectMaybePermanent(pathAndQuery, destination: $"/Facility?Id={airsNumber}");

        return RedirectMaybePermanent(pathAndQuery, destination: "");
    }

    private RedirectResult RedirectMaybePermanent(string pathAndQuery,
        [StringSyntax(StringSyntaxAttribute.Uri)] string destination)
    {
        var url = $"{AppSettings.BaseUrl}{destination}";
        logger.ZLogInformation($"Redirecting {pathAndQuery} to {url}");
        return AppSettings.IsDevelopment ? Redirect(url) : RedirectPermanent(url);
    }
}
