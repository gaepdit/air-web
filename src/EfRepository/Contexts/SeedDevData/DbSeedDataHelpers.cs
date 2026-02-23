using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Compliance.EnforcementEntities.ViolationTypes;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;
using AirWeb.TestData.Identity;
using AirWeb.TestData.Sbeap;

namespace AirWeb.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        ClearAllStaticData();
        SeedOfficeData(context);
        SeedIdentityData(context);

        SeedViolationTypeData(context);
        SeedFceData(context);
        SeedNotificationTypeData(context);
        SeedComplianceWorkData(context);
        SeedCaseFileData(context);
        SeedEnforcementActionData(context);

        SeedSbeapData(context);
    }

    private static void ClearAllStaticData()
    {
        CaseFileData.ClearData();
        EnforcementActionData.ClearData();
        FceData.ClearData();
        ComplianceWorkData.ClearData();
        NotificationTypeData.ClearData();

        ContactData.ClearData();
        CaseworkData.ClearData();
        AgencyData.ClearData();
        ActionItemData.ClearData();
        ActionItemTypeData.ClearData();
        CustomerData.ClearData();

        OfficeData.ClearData();
        UserData.ClearData();
    }

    private static void SeedViolationTypeData(AppDbContext context)
    {
        if (context.ViolationTypes.Any()) return;
        context.ViolationTypes.AddRange(ViolationTypeData.ViolationTypes);
        context.SaveChanges();
    }

    private static void SeedCaseFileData(AppDbContext context)
    {
        if (context.EnforcementCaseFiles.Any()) return;

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT CaseFiles ON");
        }

        context.EnforcementCaseFiles.AddRange(CaseFileData.GetData);
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

    private static void SeedComplianceWorkData(AppDbContext context)
    {
        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ComplianceWork ON");
        }

        if (!context.Accs.Any())
            context.Accs.AddRange(ComplianceWorkData.GetData
                .Where(work => work is AnnualComplianceCertification)
                .Cast<AnnualComplianceCertification>());

        if (!context.Inspections.Any())
            context.Inspections.AddRange(ComplianceWorkData.GetData
                .Where(work => work is Inspection)
                .Cast<Inspection>());

        if (!context.Notifications.Any())
            context.Notifications.AddRange(ComplianceWorkData.GetData
                .Where(work => work is Notification)
                .Cast<Notification>());

        if (!context.PermitRevocations.Any())
            context.PermitRevocations.AddRange(ComplianceWorkData.GetData
                .Where(work => work is PermitRevocation)
                .Cast<PermitRevocation>());

        if (!context.Reports.Any())
            context.Reports.AddRange(ComplianceWorkData.GetData
                .Where(work => work is Report)
                .Cast<Report>());

        if (!context.RmpInspections.Any())
            context.RmpInspections.AddRange(ComplianceWorkData.GetData
                .Where(work => work is RmpInspection)
                .Cast<RmpInspection>());

        if (!context.SourceTestReviews.Any())
            context.SourceTestReviews.AddRange(ComplianceWorkData.GetData
                .Where(work => work is SourceTestReview)
                .Cast<SourceTestReview>());

        context.SaveChanges();

        if (context.Database.ProviderName == AppDbContext.SqlServerProvider)
        {
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ComplianceWork OFF");
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
        if (!context.Users.Any())
            context.Users.AddRange(UserData.Users);

        // Seed Roles
        if (!context.Roles.Any())
            context.Roles.AddRange(UserData.Roles);

        // Seed User Roles
        if (!context.UserRoles.Any())
            context.UserRoles.AddRange(UserData.UserRoles);

        context.SaveChanges();
    }

    private static void SeedSbeapData(AppDbContext context)
    {
        SeedActionItemTypeData(context);
        SeedAgencyData(context);
        SeedCustomerData(context);
        SeedContactData(context);
        SeedCaseworkData(context);
        SeedActionItemData(context);
    }

    private static void SeedActionItemData(AppDbContext context)
    {
        if (context.SbeapActionItems.Any()) return;
        context.SbeapActionItems.AddRange(ActionItemData.GetActionItems);
        context.SaveChanges();
    }

    private static void SeedActionItemTypeData(AppDbContext context)
    {
        if (context.ActionItemTypes.Any()) return;
        context.ActionItemTypes.AddRange(ActionItemTypeData.GetActionItemTypes);
        context.SaveChanges();
    }

    private static void SeedAgencyData(AppDbContext context)
    {
        if (context.SbeapAgencies.Any()) return;
        context.SbeapAgencies.AddRange(AgencyData.GetAgencies);
        context.SaveChanges();
    }

    private static void SeedCaseworkData(AppDbContext context)
    {
        if (context.SbeapCases.Any()) return;
        context.SbeapCases.AddRange(CaseworkData.GetCases);
        context.SaveChanges();
    }

    private static void SeedContactData(AppDbContext context)
    {
        if (context.SbeapContacts.Any()) return;
        context.SbeapContacts.AddRange(ContactData.GetContacts(false));
        context.SaveChanges();
    }

    private static void SeedCustomerData(AppDbContext context)
    {
        if (context.SbeapCustomers.Any()) return;
        context.SbeapCustomers.AddRange(CustomerData.GetCustomers);
        context.SaveChanges();
    }
}
