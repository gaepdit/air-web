using AirWeb.Domain.Compliance;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;

public class ComplianceWorkValidator : AbstractValidator<ComplianceWorkSearchDto>
{
    private readonly IFacilityService _service;
    public ComplianceWorkValidator(IFacilityService service)
    {
        _service = service;
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.EventDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The Event From Date cannot be in the future.");

        RuleFor(dto => dto.EventDateTo)
            .Cascade(CascadeMode.Stop)
            .Must(date => date <= today || date == null)
            .WithMessage("The Event To Date cannot be in the future")
            .Must((dto, date) => dto.EventDateFrom == default || date >= dto.EventDateFrom || date == null)
            .WithMessage("The Event To Date must be later than the From Date");

        RuleFor(dto => dto.ClosedDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The Closed From Date cannot be in the future");

        RuleFor(dto => dto.ClosedDateTo)
            .Cascade(CascadeMode.Stop)
            .Must(date => date <= today || date == null)
            .WithMessage("The Closed To Date cannot be in the future")
            .Must((dto, date) => dto.ClosedDateFrom == default || date >= dto.ClosedDateFrom || date == null)
            .WithMessage("The Closed To Date must be later than the From Date");

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

                return await _service.ExistsAsync(facilityId);
            })
            .WithMessage("A Facility with that AIRS Number does not exist or has not been approved in the IAIP.");
    }
}

