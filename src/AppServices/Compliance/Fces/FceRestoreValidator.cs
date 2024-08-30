using FluentValidation;

namespace AirWeb.AppServices.Compliance.Fces;

public class FceRestoreValidator : AbstractValidator<FceRestoreDto>
{
    public FceRestoreValidator(IFceService fceService)
    {
        RuleFor(dto => dto).MustAsync(async (dto, token) =>
            !await fceService.ExistsAsync(dto, token).ConfigureAwait(false));
    }
}
