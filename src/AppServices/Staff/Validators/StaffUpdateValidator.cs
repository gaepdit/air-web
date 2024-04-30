using FluentValidation;
using MyApp.AppServices.Staff.Dto;
using MyApp.Domain.Identity;

namespace MyApp.AppServices.Staff.Validators;

[UsedImplicitly]
public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
