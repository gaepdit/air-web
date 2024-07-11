using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.Facilities;

public class FindFacility
{
    [Test]
    public async Task ReturnsFacilityIfExists()
    {
        // Arrange
        var facility = FacilityData.GetData[0];
        var repo = new LocalFacilityRepository();

        // Act
        var result = await repo.FindFacilityAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        // Arrange
        var repo = new LocalFacilityRepository();

        // Act
        var result = await repo.FindFacilityAsync((FacilityId)"000-00000");

        // Assert
        result.Should().BeNull();
    }
}
