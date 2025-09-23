using IaipDataService.SourceTests;

namespace IaipDataServiceTests.DbTests.SourceTests;

public class GetSourceTestsForFacility
{
    private IaipSourceTestService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        using var cache = Substitute.For<IMemoryCache>();
        var logger = Substitute.For<ILogger<IaipSourceTestService>>();
        _sut = new IaipSourceTestService(Config.DbConnectionFactory!, cache, logger);
    }

    [Test]
    public async Task ReturnsList()
    {
        // Act
        var result = await _sut.GetSourceTestsForFacilityAsync(Config.TestFacilityId);

        // Assert
        result.Should().NotBeEmpty();
    }
}
