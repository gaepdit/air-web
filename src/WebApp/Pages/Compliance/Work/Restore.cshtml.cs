﻿using AirWeb.AppServices.AuthorizationPolicies;
using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Query;
using AirWeb.WebApp.Models;

namespace AirWeb.WebApp.Pages.Compliance.Work;

[Authorize(Policy = nameof(Policies.ComplianceManager))]
public class RestoreModel(IWorkEntryService entryService) : PageModel
{
    [FromRoute]
    public int Id { get; set; }

    public WorkEntrySummaryDto ItemSummary { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("Index");

        var item = await entryService.FindSummaryAsync(Id);
        if (item is null) return NotFound();
        if (!User.CanRestore(item)) return Forbid();

        ItemSummary = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = await entryService.FindSummaryAsync(Id, token);
        if (item is null || !item.IsDeleted || !User.CanRestore(item))
            return BadRequest();

        var notificationResult = await entryService.RestoreAsync(Id, token);
        TempData.SetDisplayMessage(
            notificationResult.Success ? DisplayMessage.AlertContext.Success : DisplayMessage.AlertContext.Warning,
            $"{item.ItemName} successfully restored.", notificationResult.FailureMessage);

        return RedirectToPage("Details", new { Id });
    }
}
