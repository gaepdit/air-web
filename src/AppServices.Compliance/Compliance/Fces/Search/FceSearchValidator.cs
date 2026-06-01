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


