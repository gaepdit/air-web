using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class GetNameTests
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
