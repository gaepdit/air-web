using FluentValidation;
using AirWeb.AppServices.WorkEntries.CommandDto;

namespace AirWeb.AppServices.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<WorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
