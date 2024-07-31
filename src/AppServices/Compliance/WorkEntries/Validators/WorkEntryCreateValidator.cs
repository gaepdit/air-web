using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<WorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
