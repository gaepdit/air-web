using AirWeb.Domain.Identity;
using AirWeb.TestData;
using AirWeb.TestData.Identity;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.EfRepository.DbContext.DevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedOfficeData(context);
        SeedIdentityData(context);
        SeedWorkEntryData(context);
    }

    private static void SeedWorkEntryData(AppDbContext context)
    {
        if (context.WorkEntries.Any()) return;
        context.WorkEntries.AddRange(WorkEntryData.GetData);
        context.SaveChanges();
    }

    internal static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetData);
        context.SaveChanges();
    }

    public static void SeedIdentityData(AppDbContext context)
    {
        // Seed Users
        var users = UserData.GetUsers.ToList();
        if (!context.Users.Any()) context.Users.AddRange(users);

        // Seed Roles
        var roles = UserData.GetRoles.ToList();
        if (!context.Roles.Any()) context.Roles.AddRange(roles);

        // Seed User Roles
        if (!context.UserRoles.Any())
        {
            // -- admin
            var adminUserRoles = roles
                .Select(role => new IdentityUserRole<string>
                    { RoleId = role.Id, UserId = users.Single(e => e.GivenName == "Admin").Id })
                .ToList();
            context.UserRoles.AddRange(adminUserRoles);

            // -- staff
            var staffUserId = users.Single(e => e.GivenName == "General").Id;
            context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.SiteMaintenance).Id,
                    UserId = staffUserId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.Staff).Id,
                    UserId = staffUserId,
                });
        }

        context.SaveChanges();
    }
}
