using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;

public class InspectionUpdateValidator : AbstractValidator<InspectionUpdateDto>
{
    public InspectionUpdateValidator(
        IValidator<IComplianceWorkCommandDto> complianceWorkCommandValidator,
        IValidator<InspectionCommandDto> inspectionCommandValidator)
    {
        RuleFor(dto => dto).SetValidator(complianceWorkCommandValidator);
        RuleFor(dto => dto).SetValidator(inspectionCommandValidator);
    }
}
