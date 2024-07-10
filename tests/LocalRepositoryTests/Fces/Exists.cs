using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.Fces;

public class Exists
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
        var fce = FceData.GetData.First();

        // Act
        var result = await _repository.ExistsAsync(fce.Facility.Id, fce.Year);

        // Assert
        result.Should().BeTrue();
    }


    [Test]
    public async Task GivenNonexistentItem_ReturnsFalse()
    {
        // Arrange
        var fce = FceData.GetData.First();

        // Act
        var result = await _repository.ExistsAsync(fce.Facility.Id, year: 0);

        // Assert
        result.Should().BeFalse();
    }
}
