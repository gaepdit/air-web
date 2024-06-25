using AirWeb.Domain;
using AirWeb.Domain.Entities.NotificationTypes;
using FluentValidation;

namespace AirWeb.AppServices.DomainEntities.NotificationTypes.Validators;

public class NotificationTypeCreateValidator : AbstractValidator<NotificationTypeCreateDto>
{
    private readonly INotificationTypeRepository _repository;

    public NotificationTypeCreateValidator(INotificationTypeRepository repository)
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
