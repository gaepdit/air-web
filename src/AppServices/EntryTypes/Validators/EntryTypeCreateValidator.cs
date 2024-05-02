using FluentValidation;
using AirWeb.Domain;
using AirWeb.Domain.Entities.EntryTypes;

namespace AirWeb.AppServices.EntryTypes.Validators;

public class EntryTypeCreateValidator : AbstractValidator<EntryTypeCreateDto>
{
    private readonly IEntryTypeRepository _repository;

    public EntryTypeCreateValidator(IEntryTypeRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (_, name, token) => await NotDuplicateName(name, token).ConfigureAwait(false))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token).ConfigureAwait(false) is null;
}
