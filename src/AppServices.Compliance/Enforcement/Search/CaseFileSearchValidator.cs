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
    }
}
