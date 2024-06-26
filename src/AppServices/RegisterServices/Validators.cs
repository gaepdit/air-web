using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.RegisterServices;

public static class Validators
{
    // Add all validators
    public static void AddValidators(this IServiceCollection services) =>
        services.AddValidatorsFromAssemblyContaining(typeof(Validators));
}
