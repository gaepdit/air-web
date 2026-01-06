using AirWeb.AppServices.Compliance.WorkEntries.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
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
        var expected = WorkEntryData.GetData;

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void RangeMatch()
    {
        // Arrange
        var spec = WorkEntryData.GetData.First(e => e.ClosedDate != null).ClosedDate!.Value.AddDays(1);
        var expected = WorkEntryData.GetData.Where(e => e.ClosedDate <= spec);

        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ExactMatch()
    {
        // Arrange
        var spec = WorkEntryData.GetData.First(e => e.ClosedDate != null).ClosedDate!.Value;
        var expected = WorkEntryData.GetData.Where(e => e.ClosedDate <= spec);

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
        // Act
        var result = WorkEntryData.GetData.Where(GetPredicate(DateOnly.MinValue));

        // Assert
        result.Should().BeEmpty();
    }
}
