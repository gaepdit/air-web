using AirWeb.Domain.Compliance;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Enforcement.Search;

public class CaseFileSearchValidator : AbstractValidator<CaseFileSearchDto>
{
    private readonly IFacilityService _service;

    public CaseFileSearchValidator(IFacilityService service)
    {
        _service = service;
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.DiscoveryDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The Discovery Date cannot be in the future");

        RuleFor(dto => dto.DiscoveryDateTo)
            .Cascade(CascadeMode.Stop)
            .Must(date => date <= today || date == null)
            .WithMessage("The Discovery To Date cannot be in the future")
            .Must((dto, date) => dto.DiscoveryDateFrom == default || date >= dto.DiscoveryDateFrom || date == null)
            .WithMessage("The Discovery To Date must be later than the From Date");
    }
}
