using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Enforcement.Search;

public class CaseFileSearchValidator : AbstractValidator<CaseFileSearchDto>
{
    public CaseFileSearchValidator(IFacilityService service)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.DiscoveryDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning search date cannot be in the future");

        RuleFor(dto => dto.DiscoveryDateTo)
            .Must((dto, date) => date >= dto.DiscoveryDateFrom)
            .When(dto => dto.DiscoveryDateFrom.HasValue && dto.DiscoveryDateTo.HasValue)
            .WithMessage("The end search date must be later than the beginning date");

        RuleFor(dto => dto.EnforcementDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning search date cannot be in the future");

        RuleFor(dto => dto.EnforcementDateTo)
            .Must((dto, date) => date >= dto.EnforcementDateFrom)
            .When(dto => dto.EnforcementDateFrom.HasValue && dto.EnforcementDateTo.HasValue)
            .WithMessage("The end search date must be later than the beginning date");

        RuleFor(dto => dto.FacilityId)
            .Cascade(CascadeMode.Stop)
            .Must(id => string.IsNullOrWhiteSpace(id) || FacilityId.TryParse(id, out _))
            .WithMessage(FacilityId.FacilityIdFormatError)
            .MustAsync(async (id, _) =>
            {
                if (string.IsNullOrWhiteSpace(id)) return true;
                if (!FacilityId.TryParse(id, out var facilityId)) return false;
                return await service.ExistsAsync(facilityId).ConfigureAwait(false);
            })
            .WithMessage(FacilityId.FacilityNotExistsError);
    }
}
