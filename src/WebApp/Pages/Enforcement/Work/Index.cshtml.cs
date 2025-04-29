using AirWeb.AppServices.Enforcement.Permissions;
using AirWeb.AppServices.Enforcement.Search;
using AirWeb.AppServices.NamedEntities.Offices;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.Constants;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Pages.Enforcement.Work
{
    public class EnforcementIndexModel() : PageModel
    {
        public EnforcementSearchDto Spec { get; set; } = null!;
        public bool ShowResults { get; private set; }
        public bool UserCanViewDeletedRecords { get; private set; }
        public IPaginatedResult<EnforcementSearchResultDto> SearchResults { get; private set; } = null!;
        public PaginatedResultsDisplay ResultsDisplay => new(Spec, SearchResults); //add for enforce?
        public async Task OnGetAsync(CancellationToken token = default)
        {
            Spec = new EnforcementSearchDto();
        }
        public async Task OnGetSearchAsync(EnforcementSearchDto spec, [FromQuery] int p = 1,
            CancellationToken token = default)
        {
            Spec = spec;
        }
    }
}
    

