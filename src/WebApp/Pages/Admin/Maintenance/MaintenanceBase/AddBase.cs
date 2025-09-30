using AirWeb.AppServices.Lookups.LookupsBase;
using AirWeb.WebApp.Models;
using FluentValidation;

namespace AirWeb.WebApp.Pages.Admin.Maintenance.MaintenanceBase;

public abstract class AddBase : PageModel, ISubmitCancelButtons
{
    public MaintenanceOption ThisOption { get; protected set; } = null!;

    [TempData]
    public Guid HighlightId { get; set; }

    // Form buttons
    public string SubmitText => $"Add New {ThisOption.SingularName}";
    public string CancelRoute => "Index";
    public string RouteId => string.Empty;

    protected async Task<IActionResult> DoPostAsync<TViewDto, TUpdateDto, TCreateDto>(
        ILookupService<TViewDto, TUpdateDto> service,
        IValidator<TCreateDto> validator,
        TCreateDto item)
        where TCreateDto : LookupCreateDto
    {
        await validator.ApplyValidationAsync(item, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await service.CreateAsync(item.Name);
        TempData.AddDisplayMessage(DisplayMessage.AlertContext.Success, message: $"“{item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
