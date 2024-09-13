using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;

public class WorkEntryCreateValidator : AbstractValidator<IWorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.FacilityId).NotEmpty();
        RuleFor(dto => dto).SetValidator(new WorkEntryCommandValidator());
    }
}
