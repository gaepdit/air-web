using FluentValidation;
using MyApp.Domain;
using MyApp.Domain.Entities.Offices;

namespace MyApp.AppServices.Offices.Validators;

public class OfficeUpdateValidator : AbstractValidator<OfficeUpdateDto>
{
    private readonly IOfficeRepository _repository;

    public OfficeUpdateValidator(IOfficeRepository repository)
    {
        _repository = repository;

        RuleFor(updateDto => updateDto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, context, token) =>
                await NotDuplicateName(name, context, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, IValidationContext context,
        CancellationToken token = default)
    {
        var office = await _repository.FindByNameAsync(name, token).ConfigureAwait(false);
        return office is null || office.Id == (Guid)context.RootContextData["Id"];
    }
}
