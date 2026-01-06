using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceMonitoring.Search;

public class WorkTypeFilterTests
{
    private static Func<ComplianceWork, bool> GetPredicate(List<WorkTypeSearch> spec) =>
        PredicateBuilder.True<ComplianceWork>().ByWorkType(spec).Compile();

    [Test]
    public void EmptySpec_ReturnsAll()
    {
        // Arrange
        List<WorkTypeSearch> spec = [];
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void MatchSingle()
    {
        // Arrange
        List<WorkTypeSearch> spec = [WorkTypeSearch.Acc];

        var expected = WorkEntryData.GetData.Where(e =>
            e.ComplianceWorkType == ComplianceWorkType.AnnualComplianceCertification);

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void MatchMultiple()
    {
        // Arrange
        List<WorkTypeSearch> spec = [WorkTypeSearch.Acc, WorkTypeSearch.Inspection];

        var expected = WorkEntryData.GetData.Where(e =>
            e.ComplianceWorkType is ComplianceWorkType.AnnualComplianceCertification or ComplianceWorkType.Inspection);

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void MatchAll_ReturnsAll()
    {
        // Arrange
        List<WorkTypeSearch> spec =
        [
            WorkTypeSearch.Acc,
            WorkTypeSearch.Inspection,
            WorkTypeSearch.Rmp,
            WorkTypeSearch.Report,
            WorkTypeSearch.Str,
            WorkTypeSearch.Notification,
            WorkTypeSearch.PermitRevocation,
        ];
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Arrange
        List<WorkTypeSearch> spec = [(WorkTypeSearch)99];

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
