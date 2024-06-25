using AirWeb.AppServices.DomainEntities.WorkEntries.BaseWorkEntryDto;
using FluentValidation;

namespace AirWeb.AppServices.DomainEntities.WorkEntries.Validators;

public class WorkEntryCreateValidator : AbstractValidator<BaseWorkEntryCreateDto>
{
    public WorkEntryCreateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
