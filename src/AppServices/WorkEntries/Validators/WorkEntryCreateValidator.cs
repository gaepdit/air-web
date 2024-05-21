using AirWeb.AppServices.WorkEntries.BaseCommandDto;
using FluentValidation;

namespace AirWeb.AppServices.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<BaseWorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
