using AirWeb.AppServices.Compliance.WorkEntries.WorkEntryDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.WorkEntries.Reports;

public class ReportUpdateValidator : AbstractValidator<ReportUpdateDto>
{
    public ReportUpdateValidator()
    {
        RuleFor(dto => dto).SetValidator(new WorkEntryCommandValidator());
        RuleFor(dto => dto).SetValidator(new ReportCommandValidator());
    }
}
