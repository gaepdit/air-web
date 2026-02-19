using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Search;

public class MinEventDateFilterTests
{
    private static Func<ComplianceWork, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<ComplianceWork>().MinEventDate(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        DateOnly? spec = null;
        var expected = ComplianceWorkData.GetData;

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void RangeMatch()
    {
        // Arrange
        var spec = ComplianceWorkData.GetData.First().EventDate.AddDays(-1);
        var expected = ComplianceWorkData.GetData.Where(e => e.EventDate >= spec);

        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ExactMatch()
    {
        // Arrange
        var spec = ComplianceWorkData.GetData.First().EventDate;
        var expected = ComplianceWorkData.GetData.Where(e => e.EventDate >= spec);

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
        // Act
        var result = ComplianceWorkData.GetData.Where(GetPredicate(DateOnly.MaxValue));

        // Assert
        result.Should().BeEmpty();
    }
}
