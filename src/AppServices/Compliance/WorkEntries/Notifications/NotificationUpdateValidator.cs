using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Notifications;

public class NotificationUpdateValidator : AbstractValidator<NotificationUpdateDto>
{
    public NotificationUpdateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCommandValidator());
        RuleFor(dto => dto).SetValidator(new NotificationCommandValidator());
    }
}
