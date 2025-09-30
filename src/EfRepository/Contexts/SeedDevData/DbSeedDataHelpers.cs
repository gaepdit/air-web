using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;
using AirWeb.TestData.Identity;
using AirWeb.TestData.NamedEntities;

namespace AirWeb.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        ClearAllStaticData();
        SeedLookupTables(context);
        SeedOfficeData(context);
        SeedIdentityData(context);
        SeedFceData(context);
        SeedNotificationTypeData(context);
        SeedWorkEntryData(context);
        SeedCaseFileData(context);
        SeedEnforcementActionData(context);
    }

    private static void ClearAllStaticData()
    {
        CaseFileData.ClearData();
        EnforcementActionData.ClearData();
        FceData.ClearData();
        NotificationTypeData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
        WorkEntryData.ClearData();
    }

    private static void SeedLookupTables(AppDbContext context)
    {
        if (context.ViolationTypes.Any()) return;
        context.ViolationTypes.AddRange(ViolationTypeData.ViolationTypes);
        context.SaveChanges();
    }

    private static void SeedCaseFileData(AppDbContext context)
    {
        if (context.CaseFiles.Any()) return;

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CaseFiles ON");
        }

        context.CaseFiles.AddRange(CaseFileData.GetData);
        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CaseFiles OFF");
            context.Database.CommitTransaction();
        }
    }

    private static void SeedEnforcementActionData(AppDbContext context)
    {
        if (!context.AdministrativeOrders.Any())
            context.AdministrativeOrders.AddRange(EnforcementActionData.GetData.OfType<AdministrativeOrder>());

        if (!context.ConsentOrders.Any())
            context.ConsentOrders.AddRange(EnforcementActionData.GetData.OfType<ConsentOrder>());

        if (!context.InformationalLetters.Any())
            context.InformationalLetters.AddRange(EnforcementActionData.GetData.OfType<InformationalLetter>());

        if (!context.LettersOfNoncompliance.Any())
            context.LettersOfNoncompliance.AddRange(EnforcementActionData.GetData.OfType<LetterOfNoncompliance>());

        if (!context.NoFurtherActionLetters.Any())
            context.NoFurtherActionLetters.AddRange(EnforcementActionData.GetData.OfType<NoFurtherActionLetter>());

        if (!context.NoticesOfViolation.Any())
            context.NoticesOfViolation.AddRange(EnforcementActionData.GetData.OfType<NoticeOfViolation>());

        if (!context.NovNfaLetters.Any())
            context.NovNfaLetters.AddRange(EnforcementActionData.GetData.OfType<NovNfaLetter>());

        if (!context.ProposedConsentOrders.Any())
            context.ProposedConsentOrders.AddRange(EnforcementActionData.GetData.OfType<ProposedConsentOrder>());

        context.SaveChanges();
    }

    private static void SeedFceData(AppDbContext context)
    {
        if (context.Fces.Any()) return;

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Fces ON");
        }

        context.Fces.AddRange(FceData.GetData);
        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Fces OFF");
            context.Database.CommitTransaction();
        }
    }

    private static void SeedNotificationTypeData(AppDbContext context)
    {
        if (context.NotificationTypes.Any()) return;
        context.NotificationTypes.AddRange(NotificationTypeData.GetData);
        context.SaveChanges();
    }

    private static void SeedWorkEntryData(AppDbContext context)
    {
        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WorkEntries ON");
        }

        if (!context.Accs.Any())
            context.Accs.AddRange(WorkEntryData.GetData
                .Where(entry => entry is AnnualComplianceCertification)
                .Cast<AnnualComplianceCertification>());

        if (!context.Inspections.Any())
            context.Inspections.AddRange(WorkEntryData.GetData
                .Where(entry => entry is Inspection)
                .Cast<Inspection>());

        if (!context.Notifications.Any())
            context.Notifications.AddRange(WorkEntryData.GetData
                .Where(entry => entry is Notification)
                .Cast<Notification>());

        if (!context.PermitRevocations.Any())
            context.PermitRevocations.AddRange(WorkEntryData.GetData
                .Where(entry => entry is PermitRevocation)
                .Cast<PermitRevocation>());

        if (!context.Reports.Any())
            context.Reports.AddRange(WorkEntryData.GetData
                .Where(entry => entry is Report)
                .Cast<Report>());

        if (!context.RmpInspections.Any())
            context.RmpInspections.AddRange(WorkEntryData.GetData
                .Where(entry => entry is RmpInspection)
                .Cast<RmpInspection>());

        if (!context.SourceTestReviews.Any())
            context.SourceTestReviews.AddRange(WorkEntryData.GetData
                .Where(entry => entry is SourceTestReview)
                .Cast<SourceTestReview>());

        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT WorkEntries OFF");
            context.Database.CommitTransaction();
        }
    }

    private static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetData);
        context.SaveChanges();
    }

    private static void SeedIdentityData(AppDbContext context)
    {
        // Seed Users
        var users = UserData.GetData.ToList();
        if (!context.Users.Any()) context.Users.AddRange(users);

        // Seed Roles
        var roles = UserData.GetRoles.ToList();
        if (!context.Roles.Any()) context.Roles.AddRange(roles);

        context.SaveChanges();
    }
}
