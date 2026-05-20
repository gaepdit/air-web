using AirWeb.AppServices.Sbeap.Cases.Dto;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Cases.Validators;

public class CaseworkUpdateValidator : AbstractValidator<CaseworkUpdateDto>
{
    public CaseworkUpdateValidator()
    {
        RuleFor(e => e.CaseClosedDate)
            .Must((c, e) => e is null || e.Value >= c.CaseOpenedDate)
            .WithMessage("The case closure date cannot be before the date the case was opened.");

        RuleFor(e => e.ReferralDate)
            .Must((c, e) => e is null || e.Value >= c.CaseOpenedDate)
            .WithMessage("The interagency referral date cannot be before the date the case was opened.");
    }
}
