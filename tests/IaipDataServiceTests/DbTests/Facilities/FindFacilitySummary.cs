using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class FindFacilitySummary
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
    public async Task IfExists_ReturnsRecord()
    {
        // Act
        var result = await _sut.FindFacilitySummaryAsync(Config.TestFacilityId);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<Facility>();
        result.Should().NotBeNull();
        result.Id.Should().Be(Config.TestFacilityId);
    }

    [Test]
    public async Task IfNotExists_ReturnsNull()
    {
        // Act
        var result = await _sut.FindFacilitySummaryAsync(Config.NonexistentFacilityId);

        // Assert
        result.Should().BeNull();
    }
}
