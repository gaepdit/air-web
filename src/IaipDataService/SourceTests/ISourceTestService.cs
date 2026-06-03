using IaipDataService.Facilities;
using IaipDataService.SourceTests.Models;
using IaipDataService.Utilities;
using System.Text;

namespace IaipDataService.SourceTests;

public interface ISourceTestService
{
    Task<BaseSourceTestReport?> FindAsync(int referenceNumber);

    Task<SourceTestSummary?> FindSummaryAsync(int referenceNumber);

    Task<bool> SourceTestExistsAsync(int referenceNumber);

    Task<IReadOnlyCollection<SourceTestSummary>> GetSourceTestsForFacilityAsync(FacilityId facilityId);

    Task<(IReadOnlyCollection<SourceTestSummary>, int)> GetOpenSourceTestsForComplianceAsync(string? assignmentUser,
        Guid? assignmentOffice, int skip, int take);

    Task<IReadOnlyCollection<SourceTestAssignment>> GetOpenSourceTestAssignmentsAsync();

    Task UpdateSourceTestAsync(int referenceNumber, string assignmentEmail, DateOnly? reviewDate);
}

public record SourceTestAssignment
{
    public required string UserId { get; init; }
    public required string GivenName { get; init; }
    public required string FamilyName { get; init; }
    public Guid? OfficeId { get; init; }
    public string? OfficeName { get; init; }

    public string SortableNameWithOffice
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append(new[] { FamilyName, GivenName }.ConcatWithSeparator(", "));
            if (OfficeName != null) sn.Append($" — {OfficeName}");
            return sn.ToString();
        }
    }
}
