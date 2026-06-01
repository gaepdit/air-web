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

        RuleFor(dto => dto.EnforcementDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The Enforcement Date cannot be in the future");

        RuleFor(dto => dto.EnforcementDateTo)
            .Cascade(CascadeMode.Stop)
            .Must(date => date <= today || date == null)
            .WithMessage("The Enforcement To Date cannot be in the future")
            .Must((dto, date) => dto.EnforcementDateFrom == default || date >= dto.EnforcementDateFrom || date == null)
            .WithMessage("The Enforcement To Date must be later than the From Date");

        RuleFor(dto => dto.FacilityId)
            .Cascade(CascadeMode.Stop)
            .Must(id => string.IsNullOrWhiteSpace(id) ||
                FacilityId.TryParse(id, out _))
            .WithMessage(FacilityId.FacilityIdFormatError)
            .MustAsync(async (id, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return true;

                if (!FacilityId.TryParse(id, out var facilityId))
                    return false;

                return await _service.ExistsAsync(facilityId).ConfigureAwait(false);
            })
            .WithMessage("A Facility with that AIRS Number does not exist or has not been approved in the IAIP.");
    }
}
