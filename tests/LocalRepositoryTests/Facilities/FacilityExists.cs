using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.Facilities;

public class FacilityExists
{
    [Test]
    public async Task ReturnsTrueIfExists()
    {
        // Arrange
        var facility = FacilityData.GetData[0];
        var repo = new LocalFacilityRepository();

        // Act
        var result = await repo.FacilityExistsAsync(facility.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task ReturnsFalseIfNotExists()
    {
        // Arrange
        var repo = new LocalFacilityRepository();

        // Act
        var result = await repo.FacilityExistsAsync((FacilityId)"000-00000");

        // Assert
        result.Should().BeFalse();
    }
}
