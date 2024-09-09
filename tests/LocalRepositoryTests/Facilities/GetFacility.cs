using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.ExternalEntities;

namespace LocalRepositoryTests.Facilities;

public class GetFacility
{
    [Test]
    public async Task ReturnsFacilityIfExists()
    {
        // Arrange
        var facility = FacilityData.GetData[0];
        var repo = new LocalFacilityRepository();

        // Act
        var result = await repo.GetFacilityAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task ReturnsNullIfNotExists()
    {
        // Arrange
        var repo = new LocalFacilityRepository();

        // Act
        var func = async () => await repo.GetFacilityAsync((FacilityId)"000-00000");

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}
