using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.AppServices.RegisterServices;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        // Add all validators
        services.AddValidatorsFromAssemblyContaining(typeof(RegisterAppServices));
    }
}
