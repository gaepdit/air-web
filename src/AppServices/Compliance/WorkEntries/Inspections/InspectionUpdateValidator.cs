using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public class InspectionUpdateValidator : AbstractValidator<InspectionUpdateDto>
{
    public InspectionUpdateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCommandValidator());
        RuleFor(dto => dto).SetValidator(new InspectionCommandValidator());
    }
}
