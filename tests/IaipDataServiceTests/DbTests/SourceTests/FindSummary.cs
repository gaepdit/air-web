using IaipDataService.SourceTests;

namespace IaipDataServiceTests.DbTests.SourceTests;

public class FindSummary
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
    public async Task IfExists_ReturnsRecord()
    {
        // Act
        var result = await _sut.FindSummaryAsync(Config.TestFacilityReferenceNumber);

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.ReferenceNumber.Should().Be(Config.TestFacilityReferenceNumber);
        result.Facility.Should().NotBeNull();
        result.Facility.FacilityId.Should().Be(Config.TestFacilityId);
    }

    [Test]
    public async Task IfNotExists_ReturnsNull()
    {
        // Act
        var result = await _sut.FindSummaryAsync(Config.NonexistentReferenceNumber);

        // Assert
        result.Should().BeNull();
    }
}
