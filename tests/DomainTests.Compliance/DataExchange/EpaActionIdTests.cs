using AirWeb.Domain.Compliance.DataExchange;

namespace DomainTests.Compliance.DataExchange;

[TestFixture]
[TestOf(typeof(EpaActivityId))]
public class EpaActivityIdTests
{
    [Test]
    public void GivenValidId_ReturnsCorrectProperties()
    {
        // Arrange
        var result = new EpaActivityId("GA000A0000130010000256789");

        // Assert
        result.FacilityId.Id.Should().Be("00100002");
        result.ActionNumber.Should().Be(56789);
    }

    [Test]
    public void GivenInvalidActionNumber_ThrowsException()
    {
        // Arrange
        var id = "GA000A0000130010000100000";

        // Act
        var func = () => new EpaActivityId(id);

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GivenInvalidFacilityId_ThrowsException()
    {
        // Arrange
        var id = "GA000A0000130010000056789";

        // Act
        var func = () => new EpaActivityId(id);

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void GivenValidId_IsValidFormat_ReturnsTrue()
    {
        EpaActivityId.IsValidFormat("GA000A0000130010000256789").Should().BeTrue();
    }

    [Test]
    public void GivenInvalidActionNumber_IsValidFormat_ReturnsFalse()
    {
        EpaActivityId.IsValidFormat("GA000A0000130010000100000").Should().BeFalse();
    }

    [Test]
    public void GivenInvalidFacilityId_IsValidFormat_ReturnsFalse()
    {
        EpaActivityId.IsValidFormat("GA000A0000130010000056789").Should().BeFalse();
    }
}
