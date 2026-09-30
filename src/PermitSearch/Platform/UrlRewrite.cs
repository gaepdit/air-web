using Microsoft.AspNetCore.Rewrite;

namespace PermitSearch.Platform;

internal static class AppUrlRewrite
{
    // The following URLs are expected:
    // - `/permit.aspx?id={PermitFormat}`
    // - `/Permit/{PermitFormat}`
    // - `/?AirsNumber={AirsNumberFormat}`
    // - `/AirsNumber/{AirsNumberFormat}`
    // - `/`
    //
    // The query string URLs will be handled manually in the Redirect page.
    // The route URLs could be handled directly using `AddRedirect`, but instead I'm rewriting
    // them as query strings and letting the Redirect page handle them as well, to enable logging.
    // Any URLs not matching those expected will simply be redirected to the new permit search page.

    public static RewriteOptions Options => new RewriteOptions()
        .AddRewrite(regex: @"permit\.aspx", 
            replacement: "/", skipRemainingRules: true)
        .AddRewrite(regex: $"^Permit/({UrlRegex.PermitFormat})$", 
            replacement: "/?id=$1", skipRemainingRules: true)
        .AddRewrite(regex: $"^AirsNumber/({UrlRegex.AirsFormat})$",
            replacement: "/?AirsNumber=$1", skipRemainingRules: true)
        .AddRewrite(regex: ".*", replacement: "/", skipRemainingRules: true);
}
