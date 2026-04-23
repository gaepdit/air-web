using AirWeb.Domain.Compliance;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Compliance.Fces.Search;

public class FceSearchValidator : AbstractValidator<FceSearchDto>
{
    private readonly IFacilityService _service;
    public FceSearchValidator(IFacilityService service)
    {
        _service = service;
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

        RuleFor(dto => dto.FacilityId)
            .MustAsync(async (id, cancellation) =>
                await service.ExistsAsync((FacilityId)id!))
            .WithMessage("A Facility with that AIRS Number does not exist or has not been approved in the IAIP.")
            .When(dto => !string.IsNullOrWhiteSpace(dto.FacilityId));
    }

}


