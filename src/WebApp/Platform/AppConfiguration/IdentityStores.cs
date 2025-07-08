using AirWeb.Domain.Identity;
using AirWeb.EfRepository.Contexts;
using AirWeb.LocalRepository.Identity;
using AirWeb.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();

        if (AppSettings.DevSettings.BuildDatabase)
        {
            // Add EF identity stores.
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }
        else
        {
            // Add local UserStore and RoleStore.
            services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
            services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        }
    }
}
