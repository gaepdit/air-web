using AirWeb.AppServices.ExternalEntities.Facilities;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using GaEpd.AppLibrary.ListItems;

namespace AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public abstract class AddBase(IStaffService staffService) : PageModel
{
    public WorkEntryType EntryType { get; protected set; }
    public FacilityViewDto? Facility { get; protected set; }
    public SelectList StaffSelectList { get; private set; } = default!;

    protected async Task PopulateSelectListsAsync() =>
        StaffSelectList = (await staffService.GetAsListItemsAsync()).ToSelectList();
}
