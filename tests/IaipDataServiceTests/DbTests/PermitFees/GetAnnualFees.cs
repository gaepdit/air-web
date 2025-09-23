using IaipDataService.Facilities;
using IaipDataService.PermitFees;

namespace IaipDataServiceTests.DbTests.PermitFees;

public class GetAnnualFees
{
    private IaipPermitFeesService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        using var cache = Substitute.For<IMemoryCache>();
        var facilityServiceLogger = Substitute.For<ILogger<IaipFacilityService>>();
        var permitFeesServiceLogger = Substitute.For<ILogger<IaipPermitFeesService>>();
        var facilityService = new IaipFacilityService(Config.DbConnectionFactory!, cache, facilityServiceLogger);
        _sut = new IaipPermitFeesService(facilityService, Config.DbConnectionFactory!, cache, permitFeesServiceLogger);
    }

    [Test]
    public async Task IfExists_ReturnsRecord()
    {
        // Act
        var result = await _sut
            .GetAnnualFeesAsync(Config.FeesTestFacilityId, new DateOnly(2005, 10, 1), lookBackYears: 3);

        // Assert
        using var scope = new AssertionScope();
        result.Should().HaveCount(3);
        result[0].FeeYear.Should().Be(2003);
    }

    [Test]
    public async Task IfNotExists_ReturnsNull()
    {
        // Act
        var result = await _sut
            .GetAnnualFeesAsync(Config.NonexistentFacilityId, new DateOnly(2005, 10, 1), lookBackYears: 3);

        // Assert
        result.Should().BeEmpty();
    }
}
