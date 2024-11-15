using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.EnforcementEntities.Data;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AirWeb.TestData.Enforcement;

internal static class EnforcementCaseData
{
    private static IEnumerable<EnforcementCase> EnforcementCaseSeedItems =>
    [
        new(300, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Unspecified enforcement case",
            Status = EnforcementCaseStatus.CaseOpen,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
        },
        new(301, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "LON - draft",
            Status = EnforcementCaseStatus.CaseOpen,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
        },
        new(302, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "LON - open",
            Status = EnforcementCaseStatus.CaseOpen,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
        },
        new(303, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "LON - closed",
            Status = EnforcementCaseStatus.CaseClosed,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
        },
        new(304, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Unsent LON + NOV - draft",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.CaseOpen,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-9).Date),
        },
        new(305, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "NOV - no response",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.CaseOpen,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-9).Date),
        },
        new(306, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "NOV + NFA",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.CaseResolved,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-100).Date),
        },
        new(307, DomainData.GetRandomFacility().Id, null)
        {
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-60).Date),
            Notes = "Combined NOV/NFA",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.CaseResolved,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-100).Date),
        },
        new(308, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "NOV + Proposed Consent Order - draft",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.SubjectToComplianceSchedule,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(309, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Straight to Proposed CO - no response received",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.SubjectToComplianceSchedule,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(310, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Proposed CO + signed Consent Order received",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.SubjectToComplianceSchedule,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(311, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Consent Order - executed",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.SubjectToComplianceSchedule,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(312, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Consent Order + Stipulated Penalties - closed",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.CaseClosed,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-210).Date),
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddMonths(-6).Date),
        },
        new(313, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Administrative Order - executed",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.SubjectToComplianceSchedule,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-320).Date),
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddMonths(-9).Date),
        },
        new(314, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Administrative Order - closed",
            ViolationType = EnforcementData.GetRandomViolationType(),
            Status = EnforcementCaseStatus.CaseClosed,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-320).Date),
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-200).Date),
        },
        new(329, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Deleted Enforcement Case",
            Status = EnforcementCaseStatus.CaseOpen,
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-4).Date),
        },
    ];

    private static IEnumerable<EnforcementCase>? _enforcementCases;

    public static IEnumerable<EnforcementCase> GetData
    {
        get
        {
            if (_enforcementCases is not null) return _enforcementCases;
            _enforcementCases = EnforcementCaseSeedItems.ToList();

            foreach (var enforcementCase in _enforcementCases)
            {
                enforcementCase.ResponsibleStaff = UserData.GetRandomUser();
                enforcementCase.Comments.AddRange(CommentData.GetRandomCommentsList(1)
                    .Select(comment => new EnforcementCaseComment(comment, enforcementCase.Id)));

                if (enforcementCase is not { Id: > 302 }) continue;

                enforcementCase.ComplianceEvents.Add(WorkEntryData.GetRandomComplianceEvent());

                var facility = FacilityData.GetFacility(enforcementCase.FacilityId);
                if (facility.RegulatoryData is null) continue;

                enforcementCase.PollutantIds.AddRange(
                    facility.RegulatoryData.Pollutants.Select(pollutant => pollutant.Code));
                enforcementCase.AirPrograms.AddRange(facility.RegulatoryData.AirPrograms);
            }

            // Set as deleted
            _enforcementCases.Single(enforcementCase => enforcementCase.Id == 329).SetDeleted(UserData.AdminUserId);

            return _enforcementCases;
        }
    }

    public static void ClearData() => _enforcementCases = null;
}
