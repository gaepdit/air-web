using IaipDataService.Facilities;

namespace IaipDataServiceTests.DbTests.Facilities;

public class FindFacilityTests
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
        var result = await _sut.FindFacilityAsync(Config.TestFacilityId);

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
        var result = await _sut.FindFacilityAsync(Config.NonexistentFacilityId);

        // Assert
        result.Should().BeNull();
    }
}
