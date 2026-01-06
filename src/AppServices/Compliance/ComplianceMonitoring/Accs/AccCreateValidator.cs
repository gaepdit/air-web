using AirWeb.AppServices.Compliance.ComplianceMonitoring.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceMonitoring.Accs;

public class AccCreateValidator : AbstractValidator<AccCreateDto>
{
    public AccCreateValidator(
        IValidator<IWorkEntryCreateDto> workEntryCreateDtoValidator,
        IValidator<AccCommandDto> accCommandDtoValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCreateDtoValidator);
        RuleFor(dto => dto).SetValidator(accCommandDtoValidator);
    }
}
