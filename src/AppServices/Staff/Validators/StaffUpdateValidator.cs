using FluentValidation;
using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.Identity;

namespace AirWeb.AppServices.Staff.Validators;

[UsedImplicitly]
public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
