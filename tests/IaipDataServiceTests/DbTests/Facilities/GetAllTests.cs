using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class GetAllTests
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
    public async Task ReturnsList()
    {
        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        using var scope = new AssertionScope();
        result.Count.Should().BeGreaterThan(6000);
        result.Single(f => f.FacilityId == Config.TestFacilityId.FormattedId).Name.Should().Be(Config.TestFacilityName);
    }
}
