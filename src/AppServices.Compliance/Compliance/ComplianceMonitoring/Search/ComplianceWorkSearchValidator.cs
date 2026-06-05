using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;

public class ComplianceWorkSearchValidator : AbstractValidator<ComplianceWorkSearchDto>
{
    public ComplianceWorkSearchValidator(IFacilityService service)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.EventDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning search date cannot be in the future");

        RuleFor(dto => dto.EventDateTo)
            .Must((dto, date) => date >= dto.EventDateFrom)
            .When(dto => dto.EventDateFrom.HasValue && dto.EventDateTo.HasValue)
            .WithMessage("The end search date must be later than the beginning date");

        RuleFor(dto => dto.ClosedDateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning search date cannot be in the future");

        RuleFor(dto => dto.ClosedDateTo)
            .Must((dto, date) => date >= dto.ClosedDateFrom)
            .When(dto => dto.ClosedDateFrom.HasValue && dto.ClosedDateTo.HasValue)
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
