using IaipDataService.Facilities;
using Microsoft.AspNetCore.Rewrite;

namespace AirWeb.WebApp.Platform.AppConfiguration;

// URL Rewriting Middleware in ASP.NET Core
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/url-rewriting?view=aspnetcore-8.0#performance-tips-for-url-rewrite-and-redirect
public static class AppUrlRedirects
{
    // language=regex
    private const string IntRegex = @"(\d+)";

    public static IApplicationBuilder UseAppUrlRedirects(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Order rewrite rules from the most frequently matched rule to the least frequently matched rule.
        var options = new RewriteOptions()
                // Report pages
                .AddRedirect(regex: $"^facility/{FacilityId.FacilityIdRegex}/acc-report/{IntRegex}$",
                    replacement: "report/facility/$1/acc/$2",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^facility/{FacilityId.FacilityIdRegex}/stack-test/{IntRegex}$",
                    replacement: "report/facility/$1/source-test/$2",
                    statusCode: StatusCodes.Status302Found)
                .AddRedirect(regex: $"^facility/{FacilityId.FacilityIdRegex}/fce/{IntRegex}$",
                    replacement: "report/facility/$1/fce/$2",
                    statusCode: StatusCodes.Status302Found)
            ;

        return app.UseRewriter(options);
    }
}
