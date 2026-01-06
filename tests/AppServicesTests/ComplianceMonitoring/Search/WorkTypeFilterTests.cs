using AirWeb.AppServices.Compliance.ComplianceMonitoring.Search;
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
        var expected = ComplianceWorkData.GetData;

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

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

        var expected = ComplianceWorkData.GetData.Where(e =>
            e.ComplianceWorkType == ComplianceWorkType.AnnualComplianceCertification);

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

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

        var expected = ComplianceWorkData.GetData.Where(e =>
            e.ComplianceWorkType is ComplianceWorkType.AnnualComplianceCertification or ComplianceWorkType.Inspection);

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

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
        var expected = ComplianceWorkData.GetData;

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

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
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec));

        // Assert
        result.Should().BeEmpty();
    }
}
