using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.Customers;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Customers.Validators;

public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDto>
{
    public CustomerUpdateValidator(ISicCodeRepository sic)
    {
        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(Customer.MinNameLength);

        RuleFor(e => e.SicCodeId)
            .MustAsync(async (id, token) => id is null || await sic.ExistsAsync(id, token).ConfigureAwait(false))
            .WithMessage(_ => "The SIC Code entered does not exist.");
    }
}
