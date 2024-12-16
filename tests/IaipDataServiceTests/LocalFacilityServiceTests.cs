using IaipDataService.Facilities;
using IaipDataService.TestData;

namespace IaipDataServiceTests;

public class LocalFacilityServiceTests
{
    private LocalFacilityService _service = null!;

    [SetUp]
    public void SetUp() => _service = new LocalFacilityService();

    [Test]
    public async Task IfExists_FindDetails_ReturnsData()
    {
        // Arrange
        var facility = _service.Items.ElementAt(0);

        // Act
        var result = await _service.FindFacilityDetailsAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task IfNotExists_FindDetails_ReturnsNull()
    {
        // Act
        var result = await _service.FindFacilityDetailsAsync((FacilityId)"777-99999");

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task IfExists_FindSummary_ReturnsData()
    {
        // Arrange
        var facility = _service.Items.ElementAt(0);

        // Act
        var result = await _service.FindFacilitySummaryAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility);
    }

    [Test]
    public async Task IfNotExists_FindSummary_ReturnsNull()
    {
        // Act
        var result = await _service.FindFacilitySummaryAsync((FacilityId)"777-99999");

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task IfExists_GetName_ReturnsData()
    {
        // Arrange
        var facility = _service.Items.ElementAt(0);

        // Act
        var result = await _service.GetNameAsync(facility.Id);

        // Assert
        result.Should().BeEquivalentTo(facility.Name);
    }

    [Test]
    public async Task IfNotExists_GetName_Throws()
    {
        // Act
        var func = async () => await _service.GetNameAsync((FacilityId)"777-99999");

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task IfExists_Exists_ReturnsTrue()
    {
        // Arrange
        var facility = _service.Items.ElementAt(0);

        // Act
        var result = await _service.ExistsAsync(facility.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task IfNotExists_Exists_ReturnsFalse()
    {
        // Act
        var result = await _service.ExistsAsync((FacilityId)"777-99999");

        // Assert
        result.Should().BeFalse();
    }
}
