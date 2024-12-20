﻿using AirWeb.AppServices.Compliance.Permissions;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.AppServices.Permissions;
using AirWeb.AppServices.Staff;
using AirWeb.WebApp.Pages.Compliance.Work.WorkEntryBase;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Compliance.Work.PermitRevocation;

[Authorize(Policy = nameof(Policies.ComplianceStaff))]
public class EditModel(
    IWorkEntryService entryService,
    IStaffService staffService,
    IValidator<PermitRevocationUpdateDto> validator)
    : EditBase(entryService, staffService)
{
    private readonly IWorkEntryService _entryService = entryService;

    [BindProperty]
    public PermitRevocationUpdateDto Item { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == 0) return RedirectToPage("../Index");

        var item = (PermitRevocationUpdateDto?)await _entryService.FindForUpdateAsync(Id);
        if (item is null) return NotFound();
        if (!User.CanEdit(item)) return Forbid();
        Item = item;

        return await DoGetAsync();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken token)
    {
        var original = (PermitRevocationUpdateDto?)await _entryService.FindForUpdateAsync(Id, token);
        if (original is null || !User.CanEdit(original)) return BadRequest();

        return await DoPostAsync(Item, validator, token);
    }
}
