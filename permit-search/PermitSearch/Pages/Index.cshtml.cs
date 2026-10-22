using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PermitSearch.Platform;
using System.Diagnostics.CodeAnalysis;
using ZLogger;

namespace PermitSearch.Pages;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    public IActionResult OnGet([FromQuery] string? id, [FromQuery] string? airsNumber)
    {
        var pathAndQuery = Request.GetEncodedPathAndQuery();

        if (id != null && IdValidation.ValidatePermit(id, out var normalizedId))
            return RedirectMaybePermanent(pathAndQuery, destination: $"/View/{normalizedId}");

        if (airsNumber != null && IdValidation.ValidateAirs(airsNumber,  out var normalizedAirs))
            return RedirectMaybePermanent(pathAndQuery, destination: $"/Facility/{normalizedAirs}");

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
