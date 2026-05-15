using IaipDataService.SourceTests;
using IaipDataService.SourceTests.Models;

namespace IaipDataServiceTests.DbTests.SourceTests;

public class FindTests
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
    public async Task IfExists_ReturnsRecord()
    {
        // Act
        var result = await _sut.FindAsync(Config.TestFacilityReferenceNumber);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<SourceTestReportOpacity>();
        result.Should().NotBeNull();
        result.ReferenceNumber.Should().Be(Config.TestFacilityReferenceNumber);
    }

    [Test]
    public async Task IfNotExists_ReturnsNull()
    {
        // Act
        var result = await _sut.FindAsync(Config.NonexistentReferenceNumber);

        // Assert
        result.Should().BeNull();
    }
}
