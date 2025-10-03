using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.Inspections;

public class InspectionCreateValidator : AbstractValidator<InspectionCreateDto>
{
    public InspectionCreateValidator(
        IValidator<IWorkEntryCreateDto> workEntryCreateValidator,
        IValidator<InspectionCommandDto> inspectionCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(workEntryCreateValidator);
        RuleFor(dto => dto).SetValidator(inspectionCommandValidator);
    }
}
