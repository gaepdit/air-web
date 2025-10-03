using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Inspections;

public class InspectionUpdateValidator : AbstractValidator<InspectionUpdateDto>
{
    public InspectionUpdateValidator(
        IValidator<IWorkEntryCommandDto> workEntryCommandValidator,
        IValidator<InspectionCommandDto> inspectionCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
        RuleFor(dto => dto).SetValidator(inspectionCommandValidator);
    }
}
