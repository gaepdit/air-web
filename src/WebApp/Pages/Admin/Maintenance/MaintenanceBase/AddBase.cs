using AirWeb.AppServices.NamedEntities.NamedEntitiesBase;
using AirWeb.WebApp.Models;
using AirWeb.WebApp.Platform.PageModelHelpers;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

public abstract class AddBase : PageModel
{
    public MaintenanceOption ThisOption { get; protected set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    protected async Task<IActionResult> DoPost<TViewDto, TUpdateDto, TCreateDto>(
        INamedEntityService<TViewDto, TUpdateDto> service,
        IValidator<TCreateDto> validator,
        TCreateDto item)
        where TCreateDto : NamedEntityCreateDto
    {
        await validator.ApplyValidationAsync(item, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await service.CreateAsync(item.Name);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, message: $"“{item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
