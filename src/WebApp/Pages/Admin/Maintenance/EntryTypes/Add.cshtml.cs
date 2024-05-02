﻿using FluentValidation;
using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.Permissions;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.EntryTypes;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class AddModel : PageModel
{
    [BindProperty]
    public EntryTypeCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.EntryType;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IEntryTypeService service,
        [FromServices] IValidator<EntryTypeCreateDto> validator)
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        var id = await service.CreateAsync(Item.Name);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
