using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

public abstract class EditBase : PageModel
{
    [FromRoute]
    public Guid? Id { get; set; }

    public MaintenanceOption ThisOption { get; protected set; } = default!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    protected async Task<IActionResult> DoPostAsync<TViewDto, TUpdateDto>(
        INamedEntityService<TViewDto, TUpdateDto> service,
        IValidator<TUpdateDto> validator,
        TUpdateDto item)
        where TUpdateDto : NamedEntityUpdateDto
    {
        if (Id is null) return BadRequest();
        await validator.ApplyValidationAsync(item, ModelState, Id);
        if (!ModelState.IsValid) return Page();

        await service.UpdateAsync(Id.Value, item);

        HighlightId = Id.Value;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success,
            message: $"“{item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }
}
