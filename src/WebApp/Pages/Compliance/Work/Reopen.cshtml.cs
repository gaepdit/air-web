﻿using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Permissions.Helpers;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class ReopenModel(IWorkEntryService entryService, IAuthorizationService authorization) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public WorkEntrySummaryDto ItemSummary { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await entryService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!await UserCanReopenAsync(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await entryService.FindSummaryAsync(Id, token);
        if (item is null || !await UserCanReopenAsync(item))
            return BadRequest();

        var notificationResult = await entryService.ReopenAsync(Id, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            $"The {item.ItemName} has been reopened.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { Id });
    }

    private Task<bool> UserCanReopenAsync(WorkEntrySummaryDto item) =>
        authorization.Succeeded(User, item, ComplianceWorkOperation.Reopen);
}