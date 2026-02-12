using AirWeb.Core.Entities;
using AirWeb.EfRepository.Contexts;
using AirWeb.LocalRepository.Identity;
using AirWeb.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class IdentityStores
{
    public static IServiceCollection AddIdentityStores(this IServiceCollection services)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();
        services.Configure<IdentityOptions>(options => options.User.RequireUniqueEmail = true);

        if (AppSettings.DevSettings.UseDevSettings && !AppSettings.DevSettings.BuildDatabase)
        {
            // Add local UserStore and RoleStore.
            services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
            services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        }
        else
        {
            // Add EF identity stores.
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }

        return services;
    }
}
