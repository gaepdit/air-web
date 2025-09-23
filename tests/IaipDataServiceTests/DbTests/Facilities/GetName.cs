using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class GetName
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
        var result = await _sut.GetNameAsync(Config.TestFacilityId);

        // Assert
        result.Should().Be(Config.TestFacilityName);
    }

    [Test]
    public async Task IfNotExists_Throws()
    {
        // Act
        var action = async () => await _sut.GetNameAsync(Config.NonexistentFacilityId);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
