using AirWeb.AppServices.Compliance.Compliance.Fces.Search;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.Fces.Search;

public class MinCompletedDateFilterTests
{
    private static Func<Fce, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<Fce>().MinCompletedDate(spec).Compile();

    [Test]
    public void NullSpec_ReturnsAll()
    {
        // Arrange
        DateOnly? spec = null;
        var expected = FceData.GetData;

        // Act
        var result = FceData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void RangeMatch()
    {
        // Arrange
        var spec = FceData.GetData.First().CompletedDate.AddDays(-1);

        var expected = FceData.GetData.Where(e => e.CompletedDate >= spec);

        // Act
        var result = FceData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ExactMatch()
    {
        // Arrange
        var spec = FceData.GetData.First().CompletedDate;

        var expected = FceData.GetData.Where(e =>
            e.CompletedDate >= spec);

        // Act
        var result = FceData.GetData.Where(GetPredicate(spec)).ToList();

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NoMatch()
    {
        // Act
        var result = FceData.GetData.Where(GetPredicate(DateOnly.MaxValue));

        // Assert
        result.Should().BeEmpty();
    }
}
