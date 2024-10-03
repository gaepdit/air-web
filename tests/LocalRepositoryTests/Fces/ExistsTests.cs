using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Compliance;
using IaipDataService.Facilities;

namespace LocalRepositoryTests.Fces;

public class ExistsTests
{
    private LocalFceRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetFceRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var fce = FceData.GetData.First(e => !e.IsDeleted);

        // Act
        var result = await _repository.ExistsAsync((FacilityId)fce.FacilityId, fce.Year);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task IgnoringExistingItem_ReturnsFalse()
    {
        // Arrange
        var fce = FceData.GetData.First(e => !e.IsDeleted);

        // Act
        var result = await _repository.ExistsAsync((FacilityId)fce.FacilityId, fce.Year, fce.Id);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task GivenExistingItem_IgnoringDifferentItem_ReturnsTrue()
    {
        // Arrange
        var fce = FceData.GetData.First(e => !e.IsDeleted);

        // Act
        var result = await _repository.ExistsAsync((FacilityId)fce.FacilityId, fce.Year, 1);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task GivenDeletedItem_ReturnsFalse()
    {
        // Arrange
        var fce = FceData.GetData.First(e => e.IsDeleted);

        // Act
        var result = await _repository.ExistsAsync((FacilityId)fce.FacilityId, fce.Year);

        // Assert
        result.Should().BeFalse();
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
