using AirWeb.AppServices.DomainEntities.Facilities;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.TestData.Entities;

namespace AppServicesTests.Facilities;

public class FindFacility
{
    [Test]
    public async Task ReturnsFacilityDtoIfExists()
    {
        // Arrange
        var facility = FacilityData.GetData[0];
        var repoMock = Substitute.For<IFacilityRepository>();
        repoMock.FindFacilityAsync(facility.Id).Returns(facility);

        var service = new FacilityService(AppServicesTestsSetup.Mapper!, repoMock);

        // Act
        var result = await service.FindAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        // Arrange
        var repoMock = Substitute.For<IFacilityRepository>();
        repoMock.FindFacilityAsync(Arg.Any<FacilityId>()).Returns((Facility?)null);

        var service = new FacilityService(AppServicesTestsSetup.Mapper!, repoMock);

        // Act
        var result = await service.FindAsync((FacilityId)"000-00000");

        // Assert
        result.Should().BeNull();
    }
}
