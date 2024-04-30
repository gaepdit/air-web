using FluentValidation;
using MyApp.Domain;
using MyApp.Domain.Entities.EntryTypes;

namespace MyApp.AppServices.EntryTypes.Validators;

public class EntryTypeUpdateValidator : AbstractValidator<EntryTypeUpdateDto>
{
    private readonly IEntryTypeRepository _repository;

    public EntryTypeUpdateValidator(IEntryTypeRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
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
        var item = await _repository.FindByNameAsync(name, token).ConfigureAwait(false);
        return item is null || item.Id == (Guid)context.RootContextData["Id"];
    }
}
