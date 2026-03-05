using AirWeb.Domain.Core.Data.DataAttributes;

namespace DomainTests.Core.DataAttributes;

public class MaxDateAttributeTests
{
    [Test]
    public void Constructor_WithNoParameters_SetsUseTodayAsMaxToTrue()
    {
        // Arrange & Act
        var attribute = new MaxDateAttribute();

        // Assert
        attribute.MaxDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
        attribute.MaxDateValue.Should().Be(
            DateOnly.FromDateTime(DateTime.Now).ToString(MaxDateAttribute.HtmlInputDate));
    }

    [Test]
    public void Constructor_WithSpecificDate_SetsMaxDate()
    {
        // Arrange
        const int year = 2025;
        const int month = 12;
        const int day = 31;

        // Act
        var attribute = new MaxDateAttribute(year, month, day);

        // Assert
        attribute.MaxDate.Should().Be(new DateOnly(year, month, day));
        attribute.MaxDateValue.Should().Be(
            new DateOnly(year, month, day).ToString(MaxDateAttribute.HtmlInputDate));
    }

    [Test]
    public void Constructor_WithDayDelta_SetsMaxDate()
    {
        // Arrange
        const int delta = 2;

        // Act
        var attribute = new MaxDateAttribute(delta);

        // Assert
        attribute.MaxDateValue.Should().Be(
            DateOnly.FromDateTime(DateTime.Now).AddDays(delta).ToString(MaxDateAttribute.HtmlInputDate));
    }

    [Test]
    public void Constructor_WithSpecificDate_ThrowsForInvalidDate()
    {
        // Arrange & Act
        Action act = () => _ = new MaxDateAttribute(2025, 13, 1); // Invalid month

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
