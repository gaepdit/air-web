using FluentValidation;

namespace AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;

public class WorkEntryCreateValidator : AbstractValidator<IWorkEntryCreateDto>
{
    public WorkEntryCreateValidator(IValidator<IWorkEntryCommandDto> workEntryCommandValidator)
    {
        RuleFor(dto => dto.FacilityId).NotEmpty();
        RuleFor(dto => dto).SetValidator(workEntryCommandValidator);
    }
}
