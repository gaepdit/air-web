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
            .WithMessage("The FCE From Date cannot be in the future.");

        RuleFor(dto => dto.DateTo)
            .Must((dto, date) => date >= dto.DateFrom)
            .When(dto => dto.DateFrom.HasValue && dto.DateTo.HasValue)
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

                return await service.ExistsAsync(facilityId).ConfigureAwait(false);
            })
            .WithMessage("A Facility with that AIRS Number does not exist or has not been approved in the IAIP.");
    }
}


