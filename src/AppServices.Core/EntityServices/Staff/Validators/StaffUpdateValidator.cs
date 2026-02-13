using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Core.Entities;
using FluentValidation;

namespace AirWeb.AppServices.Core.EntityServices.Staff.Validators;

[UsedImplicitly]
public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
