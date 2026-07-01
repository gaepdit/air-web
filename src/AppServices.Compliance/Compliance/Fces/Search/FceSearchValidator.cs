using FluentValidation;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Compliance.Fces.Search;

public class FceSearchValidator : AbstractValidator<FceSearchDto>
{
    public FceSearchValidator(IFacilityService service)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        RuleFor(dto => dto.DateFrom)
            .Must(date => date <= today || date == null)
            .WithMessage("The beginning search date cannot be in the future.");

        RuleFor(dto => dto.DateTo)
            .Must((dto, date) => date >= dto.DateFrom)
            .When(dto => dto.DateFrom.HasValue && dto.DateTo.HasValue)
            .WithMessage("The end search date must be later than the beginning date.");

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
