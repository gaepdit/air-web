using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class FacilityExistsTests
{
    private IaipFacilityService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _sut = new IaipFacilityService(Config.DbConnectionFactory!, Setup.FakeCache!,
            Substitute.For<ILogger<IaipFacilityService>>());
    }

    [Test]
    public async Task IfExists_ReturnsTrue()
    {
        // Act
        var result = await _sut.ExistsAsync(Config.TestFacilityId);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task IfNotExists_ReturnsFalse()
    {
        // Act
        var result = await _sut.ExistsAsync(Config.NonexistentFacilityId);

        // Assert
        result.Should().BeFalse();
    }
}
