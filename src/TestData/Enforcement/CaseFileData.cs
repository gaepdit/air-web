using AirWeb.Domain.EnforcementEntities.Cases;
using AirWeb.Domain.EnforcementEntities.ViolationTypes;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;
using IaipDataService.TestData;

namespace AirWeb.TestData.Enforcement;

internal static class CaseFileData
{
    private static ViolationType GetRandomViolationType() =>
        ViolationTypeData.ViolationTypes.Where(type => !type.Deprecated).OrderBy(_ => Guid.NewGuid()).First();

    private static IEnumerable<CaseFile> CaseFileSeedItems =>
    [
        new(300, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Unspecified enforcement case",
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-1).Date),
        },
        new(301, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "LON - draft",
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
        },
        new(302, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "LON - open",
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(-2).Date),
        },
        new(303, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "LON - closed",
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-10).Date),
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-3).Date),
            ClosedBy = UserData.GetRandomUser(),
        },
        new(304, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Unsent LON + NOV - draft",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-9).Date),
        },
        new(305, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "NOV - no response",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-9).Date),
        },
        new(306, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "NOV + NFA",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-100).Date),
        },
        new(307, DomainData.GetRandomFacility().Id, null)
        {
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-60).Date),
            ClosedBy = UserData.GetRandomUser(),
            Notes = "Combined NOV/NFA",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-3).AddDays(-100).Date),
        },
        new(308, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "NOV + Proposed Consent Order - draft",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(309, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Straight to Proposed CO - no response received",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(310, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Proposed CO + signed Consent Order received",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(311, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Consent Order - executed",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(41).Date),
        },
        new(312, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Consent Order + Stipulated Penalties - closed",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-4).AddDays(-210).Date),
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddMonths(-6).Date),
        },
        new(313, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Administrative Order - executed",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-320).Date),
        },
        new(314, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Administrative Order - closed",
            ViolationType = GetRandomViolationType(),
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-320).Date),
            ClosedDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-2).AddDays(-200).Date),
        },
        new(329, DomainData.GetRandomFacility().Id, null)
        {
            Notes = "Deleted Enforcement Case",
            DiscoveryDate = DateOnly.FromDateTime(DateTimeOffset.Now.AddYears(-1).AddDays(-4).Date),
        },
    ];

    private static IEnumerable<CaseFile>? _caseFiles;

    public static IEnumerable<CaseFile> GetData
    {
        get
        {
            if (_caseFiles is not null) return _caseFiles;
            _caseFiles = CaseFileSeedItems.ToList();

            foreach (var caseFile in _caseFiles)
            {
                caseFile.ResponsibleStaff = UserData.GetRandomUser();
                caseFile.Comments.AddRange(CommentData.GetRandomCommentsList(1)
                    .Select(comment => new CaseFileComment(comment, caseFile.Id)));

                if (caseFile is not { Id: > 302 }) continue;

                var randomComplianceEvent = WorkEntryData.GetRandomComplianceEvent((FacilityId)caseFile.FacilityId);
                if (randomComplianceEvent != null)
                {
                    caseFile.ComplianceEvents.Add(randomComplianceEvent);
                    randomComplianceEvent.CaseFiles.Add(caseFile);
                }

                var facility = FacilityData.GetFacility(caseFile.FacilityId);
                if (facility.RegulatoryData is null) continue;

                caseFile.PollutantIds.AddRange(
                    facility.RegulatoryData.Pollutants.Select(pollutant => pollutant.Code));
                caseFile.AirPrograms.AddRange(facility.RegulatoryData.AirPrograms);
            }

            // Set as deleted
            _caseFiles.Single(caseFile => caseFile.Id == 329).SetDeleted(UserData.AdminUserId);

            return _caseFiles;
        }
    }

    public static void ClearData() => _caseFiles = null;
}
