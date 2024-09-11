using AirWeb.AppServices.Staff;
using AirWeb.AppServices.Users;
using AirWeb.Domain.Identity;
using AirWeb.EfRepository.DbContext;
using AirWeb.LocalRepository.Identity;
using AirWeb.WebApp.Platform.Settings;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.WebApp.Platform.AppConfiguration;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();

        // When running locally, you have the option to use in-memory data or a database.
        if (AppSettings.DevSettings.UseInMemoryData)
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

        // Add staff and user services.
        services.AddTransient<IStaffService, StaffService>();
        services.AddScoped<IUserService, UserService>();
    }
}
