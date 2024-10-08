﻿using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.Identity;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Identity;
using AirWeb.TestData.NamedEntities;
using Microsoft.AspNetCore.Identity;

namespace AirWeb.EfRepository.DbContext.DevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedOfficeData(context);
        SeedIdentityData(context);
        SeedFceData(context);
        SeedNotificationTypeData(context);
        SeedWorkEntryData(context);
    }

    internal static void SeedFceData(AppDbContext context)
    {
        if (context.Fces.Any()) return;

        context.Database.BeginTransaction();
        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Fces ON");

        context.Fces.AddRange(FceData.GetData);
        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Fces OFF");
        context.Database.CommitTransaction();
    }

    internal static void SeedNotificationTypeData(AppDbContext context)
    {
        if (context.NotificationTypes.Any()) return;
        context.NotificationTypes.AddRange(NotificationTypeData.GetData);
        context.SaveChanges();
    }

    private static void SeedWorkEntryData(AppDbContext context)
    {
        context.Database.BeginTransaction();
        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WorkEntries ON");

        if (!context.Accs.Any())
        {
            context.Accs.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.AnnualComplianceCertification)
                .Cast<AnnualComplianceCertification>());
        }

        if (!context.Inspections.Any())
        {
            context.Inspections.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.Inspection)
                .Cast<Inspection>());
        }

        if (!context.Notifications.Any())
        {
            context.Notifications.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.Notification)
                .Cast<Notification>());
        }

        if (!context.PermitRevocations.Any())
        {
            context.PermitRevocations.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.PermitRevocation)
                .Cast<PermitRevocation>());
        }

        if (!context.Reports.Any())
        {
            context.Reports.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.Report)
                .Cast<Report>());
        }

        if (!context.RmpInspections.Any())
        {
            context.RmpInspections.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.RmpInspection)
                .Cast<RmpInspection>());
        }

        if (!context.SourceTestReviews.Any())
        {
            context.SourceTestReviews.AddRange(WorkEntryData.GetData
                .Where(entry => entry.WorkEntryType == WorkEntryType.SourceTestReview)
                .Cast<SourceTestReview>());
        }

        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WorkEntries OFF");
        context.Database.CommitTransaction();
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
                    RoleId = roles.Single(e => e.Name == RoleName.ComplianceSiteMaintenance).Id,
                    UserId = staffUserId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.ComplianceStaff).Id,
                    UserId = staffUserId,
                });
        }

        context.SaveChanges();
    }
}
