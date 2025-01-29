using IaipDataService.Facilities;
using Microsoft.AspNetCore.Rewrite;

namespace AirWeb.WebApp.Platform.AppConfiguration;

// URL Rewriting Middleware in ASP.NET Core
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/url-rewriting#performance-tips-for-url-rewrite-and-redirect
public static class AppUrlRedirects
{
    // language=regex
    private const string IntPattern = @"(\d+)";

    public static IApplicationBuilder UseAppUrlRedirects(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Order rewrite rules from the most frequently matched rule to the least frequently matched rule.
        var options = new RewriteOptions()
                // Report pages
                .AddRedirect(regex: $"^facility/{FacilityId.FacilityIdPattern}/acc-report/{IntPattern}$",
                    replacement: "print/acc/$1",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^facility/{FacilityId.FacilityIdPattern}/stack-test/{IntPattern}$",
                    replacement: "print/source-test/$1",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^facility/{FacilityId.FacilityIdPattern}/fce/{IntPattern}$",
                    replacement: "print/fce/$1",
                    statusCode: StatusCodes.Status302Found)
            ;

        return app.UseRewriter(options);
    }
}
