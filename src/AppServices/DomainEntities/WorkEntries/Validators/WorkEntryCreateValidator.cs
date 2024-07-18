using AirWeb.AppServices.DomainEntities.WorkEntries.WorkEntryDto;
using FluentValidation;

namespace AirWeb.AppServices.DomainEntities.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<WorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
