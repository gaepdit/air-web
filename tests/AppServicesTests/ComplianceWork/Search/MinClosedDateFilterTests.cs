﻿using AirWeb.AppServices.Compliance.ComplianceWork.Search;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Compliance;
using GaEpd.AppLibrary.Domain.Predicates;

namespace AppServicesTests.ComplianceWork.Search;

public class MinClosedDateFilterTests
{
    private static Func<WorkEntry, bool> GetPredicate(DateOnly? spec) =>
        PredicateBuilder.True<WorkEntry>().MinClosedDate(spec).Compile();

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
        var spec = WorkEntryData.GetData.First(e => e.ClosedDate != null).ClosedDate!.Value.AddDays(-1);
        var expected = WorkEntryData.GetData.Where(e => e.ClosedDate >= spec);

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
        var expected = WorkEntryData.GetData.Where(e => e.ClosedDate >= spec);

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
        var result = WorkEntryData.GetData.Where(GetPredicate(DateOnly.MaxValue));

        // Assert
        result.Should().BeEmpty();
    }
}
