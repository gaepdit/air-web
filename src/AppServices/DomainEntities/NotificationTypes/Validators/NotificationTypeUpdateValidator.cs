using AirWeb.Domain;
using AirWeb.Domain.Entities.NotificationTypes;
using FluentValidation;

namespace AirWeb.AppServices.DomainEntities.NotificationTypes.Validators;

public class NotificationTypeUpdateValidator : AbstractValidator<NotificationTypeUpdateDto>
{
    private readonly INotificationTypeRepository _repository;

    public NotificationTypeUpdateValidator(INotificationTypeRepository repository)
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
