using IaipDataService.Facilities;

namespace IaipDataServiceTests;

public class LocalFacilityServiceTests
{
    private LocalFacilityService _service = default!;

    [SetUp]
    public void SetUp() => _service = new LocalFacilityService();

    [Test]
    public async Task IfExists_Find_ReturnsData()
    {
        // Arrange
        var facility = FacilityData.GetData[0];

        // Act
        var result = await _service.FindAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task IfNotExists_Find_ReturnsNull()
    {
        // Act
        var result = await _service.FindAsync((FacilityId)"000-00000");

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task IfExists_Get_ReturnsData()
    {
        // Arrange
        var facility = FacilityData.GetData[0];

        // Act
        var result = await _service.GetAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task IfNotExists_Get_Throws()
    {
        // Act
        var func = async () => await _service.GetAsync((FacilityId)"000-00000");

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task IfExists_Exists_ReturnsTrue()
    {
        // Arrange
        var facility = FacilityData.GetData[0];

        // Act
        var result = await _service.ExistsAsync(facility.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task IfNotExists_Exists_ReturnsFalse()
    {
        // Act
        var result = await _service.ExistsAsync((FacilityId)"000-00000");

        // Assert
        result.Should().BeFalse();
    }
}
