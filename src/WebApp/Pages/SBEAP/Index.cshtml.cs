using AirWeb.Domain.Core.Entities;
using Microsoft.AspNetCore.Identity;

// using Sbeap.AppServices.AuthorizationPolicies;
// using Sbeap.AppServices.Cases;
// using Sbeap.AppServices.Cases.Dto;
// using Sbeap.Domain.Identity;
// using Sbeap.WebApp.Platform.Constants;

namespace AirWeb.WebApp.Pages.SBEAP;

[AllowAnonymous]
public class IndexModel(
    SignInManager<ApplicationUser> signInManager,
    IAuthorizationService authorization
    // ICaseworkService cases
)
    : PageModel
{
    // Properties
    public bool ShowDashboard { get; private set; } = true;
    // public IPaginatedResult<CaseworkSearchResultDto> OpenCases { get; private set; } = null!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        if (!signInManager.IsSignedIn(User)) return LocalRedirect("~/Account/Login");

        // ShowDashboard = await UseDashboardAsync();
        if (!ShowDashboard) return Page();

        // Load dashboard modules
        // var openCasesSpec = new CaseworkSearchDto { Status = CaseStatus.Open, Sort = CaseworkSortBy.OpenedDate };
        // var paging = new PaginatedRequest(1, GlobalConstants.PageSize, openCasesSpec.Sort.GetDescription());
        // OpenCases = await cases.SearchAsync(openCasesSpec, paging);

        return Page();
    }

    // private async Task<bool> UseDashboardAsync() =>
    // (await authorization.AuthorizeAsync(User, nameof(Policies.StaffUser))).Succeeded;
}
