using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Inspections;

public class InspectionCreateValidator : AbstractValidator<InspectionCreateDto>
{
    public InspectionCreateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCreateValidator());
        RuleFor(dto => dto).SetValidator(new InspectionCommandValidator());
    }
}
