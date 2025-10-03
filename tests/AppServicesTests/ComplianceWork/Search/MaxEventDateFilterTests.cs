using AirWeb.AppServices.Compliance.ComplianceWork.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceWork.Search;

public class MaxEventDateFilterTests
{
    private static Func<WorkEntry, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<WorkEntry>().MaxEventDate(spec).Compile();

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
        var spec = WorkEntryData.GetData.First().EventDate.AddDays(1);
        var expected = WorkEntryData.GetData.Where(e => e.EventDate <= spec);

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
        var spec = WorkEntryData.GetData.First().EventDate;
        var expected = WorkEntryData.GetData.Where(e => e.EventDate <= spec);

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
