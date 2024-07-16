using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.Fces;

public class Exists
{
    private IFceRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetFceRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var fce = FceData.GetData.First();

        // Act
        var result = await _repository.ExistsAsync((FacilityId)fce.FacilityId, fce.Year);

        // Assert
        result.Should().BeTrue();
    }


    [Test]
    public async Task GivenNonexistentItem_ReturnsFalse()
    {
        // Arrange
        var fce = FceData.GetData.First();

        // Act
        var result = await _repository.ExistsAsync((FacilityId)fce.FacilityId, year: 0);

        // Assert
        result.Should().BeFalse();
    }
}
