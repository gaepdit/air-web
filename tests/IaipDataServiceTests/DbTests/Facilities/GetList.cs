using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class GetList
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
        var result = await _sut.GetListAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Count.Should().BeGreaterThan(6000);
        result[Config.TestFacilityId].Should().Be(Config.TestFacilityName);
    }
}
