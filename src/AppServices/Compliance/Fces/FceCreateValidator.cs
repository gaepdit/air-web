using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using FluentValidation;

namespace AirWeb.AppServices.Compliance.Fces;

public class FceCreateValidator : AbstractValidator<FceCreateDto>
{
    private readonly IFceRepository _repository;

    public FceCreateValidator(IFceRepository repository)
    {
        _repository = repository;

        RuleFor(dto => dto.FacilityId)
            .Cascade(CascadeMode.Stop)
            .NotNull();

        RuleFor(dto => dto)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (dto, token) =>
                await UniqueFacilityYear(dto.FacilityId!, dto.Year, token).ConfigureAwait(false))
            .WithMessage("An FCE already exists for that facility and year.");
    }

    private async Task<bool> UniqueFacilityYear(FacilityId facilityId, int year, CancellationToken token = default) =>
        !await _repository.ExistsAsync(facilityId, year, token).ConfigureAwait(false);
}
