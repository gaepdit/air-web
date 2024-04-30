using FluentValidation;
using MyApp.AppServices.WorkEntries.CommandDto;

namespace MyApp.AppServices.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<WorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
