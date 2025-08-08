using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Accs;

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
