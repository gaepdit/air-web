using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceMonitoring.Search;

public class MaxClosedDateFilterTests
{
    private static Func<ComplianceWork, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<ComplianceWork>().MaxClosedDate(spec).Compile();

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
        var spec = ComplianceWorkData.GetData.First(e => e.ClosedDate != null).ClosedDate!.Value.AddDays(1);
        var expected = ComplianceWorkData.GetData.Where(e => e.ClosedDate <= spec);

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
        var spec = ComplianceWorkData.GetData.First(e => e.ClosedDate != null).ClosedDate!.Value;
        var expected = ComplianceWorkData.GetData.Where(e => e.ClosedDate <= spec);

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
        var result = ComplianceWorkData.GetData.Where(GetPredicate(DateOnly.MinValue));

        // Assert
        result.Should().BeEmpty();
    }
}
