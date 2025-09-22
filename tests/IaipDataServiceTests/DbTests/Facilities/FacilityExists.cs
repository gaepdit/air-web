using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class FacilityExists
{
    private IaipFacilityService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        using var cache = Substitute.For<IMemoryCache>();
        var logger = Substitute.For<ILogger<IaipFacilityService>>();
        _sut = new IaipFacilityService(Config.DbConnectionFactory!, cache, logger);
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
