using IaipDataService.Facilities;
using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace IaipDataService.TestData;

public class TestSourceTestService : ISourceTestService
{
    internal IReadOnlyCollection<BaseSourceTestReport> Items { get; } = SourceTestData.GetData.ToList();

    public Task<BaseSourceTestReport?> FindAsync(int referenceNumber)
    {
        var result = Items.SingleOrDefault(report => report.ReferenceNumber == referenceNumber);
        result?.ParseConfidentialParameters();
        return Task.FromResult(result);
    }

    public Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber)
    {
        var result = Items.SingleOrDefault(report => report.ReferenceNumber == referenceNumber);
        return Task.FromResult(result is null ? null : new SourceTestSummary(result));
    }

    public Task<bool> SourceTestExistsAsync(int referenceNumber) =>
        Task.FromResult(Items.Any(report => report.ReferenceNumber == referenceNumber));

    public Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId) =>
        Task.FromResult<IReadOnlyCollection<SourceTestSummary>>(Items
            .Where(report => report.Facility?.FacilityId == facilityId)
            .OrderByDescending(report => report.TestDates.StartDate)
            .ThenByDescending(report => report.ReferenceNumber)
            .Select(report => new SourceTestSummary(report)).ToList());

    private readonly Guid _officeId = new("10000000-0000-0000-0000-000000000011");

    // While the `IaipSourceTestService` can join the `AirWeb` database, this class does not have access to
    // the test office data, so all tests are assumed to be assigned to one office for demonstration purposes.
    public Task<(IReadOnlyCollection<SourceTestSummary>, int)> GetOpenSourceTestsForComplianceAsync(
        string? assignmentUser, Guid? assignmentOffice, int skip, int take)
    {
        var assignedOffice = _officeId;
        var staff = StaffData.GetData.SingleOrDefault(s => s.IdAsGuid.ToString() == assignmentUser);

        var allTests = Items
            .Where(report => report is { ReportClosed: true, IaipComplianceComplete: false })
            .Where(report => assignmentUser is null || report.IaipComplianceAssignment == staff.EmailAddress)
            .Where(_ => assignmentOffice is null || assignmentOffice == assignedOffice)
            .OrderByDescending(report => report.DateTestReviewComplete)
            .ThenByDescending(report => report.ReferenceNumber).ToList();

        var testsPage = allTests.Skip(skip).Take(take).Select(report => new SourceTestSummary(report)).ToList();

        return Task.FromResult<(IReadOnlyCollection<SourceTestSummary>, int)>((testsPage, allTests.Count));
    }

    // For demonstration purposes, this method returns all active users and only one office.
    public Task<IReadOnlyCollection<SourceTestAssignment>> GetOpenSourceTestAssignmentsAsync() =>
        Task.FromResult<IReadOnlyCollection<SourceTestAssignment>>(StaffData.GetData.Select(staff =>
            new SourceTestAssignment
            {
                UserId = staff.IdAsGuid.ToString(),
                GivenName = staff.Name.GivenName,
                FamilyName = staff.Name.FamilyName,
                OfficeId = _officeId,
                OfficeName = "Branch Office",
            }).ToList());

    public Task UpdateSourceTestAsync(int referenceNumber, string assignmentEmail, DateOnly? reviewDate)
    {
        var report = Items.Single(report => report.ReferenceNumber == referenceNumber);
        report.IaipComplianceAssignment = assignmentEmail;
        report.IaipComplianceComplete = reviewDate != null;

        return Task.CompletedTask;
    }
}
