using FluentValidation;

namespace AirWeb.AppServices.Enforcement.Command;

public class CaseFileUpdateValidator : AbstractValidator<CaseFileUpdateDto>
{
    public CaseFileUpdateValidator()
    {
        RuleFor(dto => dto.ResponsibleStaffId).NotEmpty();
        RuleFor(dto => dto.DiscoveryDate)
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("The Discovery Date cannot be in the future.");
    }
}
