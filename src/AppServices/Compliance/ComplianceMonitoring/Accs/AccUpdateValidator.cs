using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;

public class AccUpdateValidator : AbstractValidator<AccUpdateDto>
{
    public AccUpdateValidator(
        IValidator<IWorkEntryCommandDto> workEntryCommandValidator,
        IValidator<AccCommandDto> accCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
        RuleFor(dto => dto).SetValidator(accCommandValidator);
    }
}
