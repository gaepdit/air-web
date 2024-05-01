﻿using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.AppServices.WorkEntries;
using AirWeb.AppServices.WorkEntries.QueryDto;
using AirWeb.Domain.Entities.WorkEntries;

namespace AirWeb.WebApp.Pages.Staff;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class DashboardIndexModel(IWorkEntryService workEntryService, IAuthorizationService authorization) : PageModel
{
    public bool IsStaff { get; private set; }
    public DashboardCard OpenWorkEntries { get; private set; } = null!;

    public async Task<PageResult> OnGetAsync(CancellationToken token)
    {
        IsStaff = await authorization.Succeeded(User, Policies.StaffUser);

        if (!IsStaff) return Page();

        var spec = new WorkEntrySearchDto { Status = WorkEntryStatus.Open };
        var paging = new PaginatedRequest(1, 5, SortBy.ReceivedDateDesc.GetDescription());
        OpenWorkEntries = new DashboardCard("Recent Open Work Entries")
            { WorkEntries = (await workEntryService.SearchAsync(spec, paging, token)).Items.ToList() };

        return Page();
    }

    public record DashboardCard(string Title)
    {
        public required IReadOnlyCollection<WorkEntrySearchResultDto> WorkEntries { get; init; }
        public string CardId => Title.ToLowerInvariant().Replace(oldChar: ' ', newChar: '-');
    }
}
