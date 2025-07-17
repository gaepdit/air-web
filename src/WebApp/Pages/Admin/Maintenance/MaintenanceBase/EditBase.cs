using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using AirWeb.WebApp.Models;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

public abstract class EditBase : PageModel, ISubmitCancelButtons
{
    [FromRoute]
    public Guid? Id { get; set; }

    public MaintenanceOption ThisOption { get; protected set; } = null!;

    [BindProperty]
    public string OriginalName { get; set; } = string.Empty;

    [TempData]
    public Guid HighlightId { get; set; }

    // Form buttons
    public string SubmitText => "Save Changes";
    public string CancelRoute => "Index";
    public string RouteId => string.Empty;

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
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success,
            message: $"“{item.Name}” successfully updated.");
        return RedirectToPage("Index");
    }
}
