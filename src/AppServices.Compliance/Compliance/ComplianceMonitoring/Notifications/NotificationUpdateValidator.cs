using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;

public class NotificationUpdateValidator : AbstractValidator<NotificationUpdateDto>
{
    public NotificationUpdateValidator(
        IValidator<IComplianceWorkCommandDto> complianceWorkCommandValidator,
        IValidator<NotificationCommandDto> notificationCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCommandValidator);
        RuleFor(dto => dto).SetValidator(notificationCommandValidator);
    }
}
