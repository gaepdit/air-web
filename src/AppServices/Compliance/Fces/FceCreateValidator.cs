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
            .WithMessage("An FCE already exists for that facility and year.")
            .Must(fce => ValidFceYear(fce.Year))
            .WithMessage("An FCE cannot be created for the selected year.");
    }

    private static bool ValidFceYear(int fceYear) => Fce.ValidFceYears.Contains(fceYear);

    private async Task<bool> UniqueFacilityYear(string facilityId, int year, CancellationToken token = default) =>
        !await _repository.ExistsAsync((FacilityId)facilityId, year, token).ConfigureAwait(false);
}
