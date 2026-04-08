using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class GetAllTests
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
    public async Task ReturnsList()
    {
        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Count.Should().BeGreaterThan(6000);
        result.Single(f => f.Id == Config.TestFacilityId).Name.Should().Be(Config.TestFacilityName);
    }
}
