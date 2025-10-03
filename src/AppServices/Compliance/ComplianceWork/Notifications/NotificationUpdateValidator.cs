using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Notifications;

public class NotificationUpdateValidator : AbstractValidator<NotificationUpdateDto>
{
    public NotificationUpdateValidator(
        IValidator<IWorkEntryCommandDto> workEntryCommandValidator,
        IValidator<NotificationCommandDto> notificationCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
        RuleFor(dto => dto).SetValidator(notificationCommandValidator);
    }
}
