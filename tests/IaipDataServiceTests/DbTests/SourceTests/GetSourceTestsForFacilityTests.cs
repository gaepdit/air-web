using IaipDataService.SourceTests;

namespace IaipDataServiceTests.DbTests.SourceTests;

public class GetSourceTestsForFacilityTests
{
    private IaipSourceTestService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _sut = new IaipSourceTestService(Config.DbConnectionFactory!, Setup.FakeCache!,
            Substitute.For<ILogger<IaipSourceTestService>>());
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
