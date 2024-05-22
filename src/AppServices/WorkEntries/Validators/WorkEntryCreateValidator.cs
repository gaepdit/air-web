using AirWeb.AppServices.WorkEntries.BaseWorkEntryDto;
using FluentValidation;

namespace AirWeb.AppServices.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<BaseWorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
