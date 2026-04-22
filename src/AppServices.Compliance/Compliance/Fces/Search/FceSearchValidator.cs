using AirWeb.Domain.Compliance;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Compliance.Fces.Search;

public class FceSearchValidator : AbstractValidator<FceSearchDto>
{

    public FceSearchValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.DateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The FCE From Date cannot be in the future.");

        RuleFor(dto => dto.DateTo)
            .Cascade(CascadeMode.Stop)
            .Must(date => date <= today || date == null)
            .WithMessage("The FCE To Date cannot be in the future.")
            .Must((dto, date) => dto.DateFrom == default || date >= dto.DateFrom || date == null)
            .WithMessage("The FCE To Date must be later than the From Date.");
    }

}


