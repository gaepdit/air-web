using AirWeb.AppServices.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;

public class NotificationCreateValidator : AbstractValidator<NotificationCreateDto>
{
    public NotificationCreateValidator(
        IValidator<IComplianceWorkCreateDto> complianceWorkCreateValidator,
        IValidator<NotificationCommandDto> notificationCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCreateValidator);
        RuleFor(dto => dto).SetValidator(notificationCommandValidator);
    }
}
