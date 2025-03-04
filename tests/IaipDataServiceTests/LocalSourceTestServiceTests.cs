using IaipDataService.TestData;

namespace IaipDataServiceTests;

public class LocalSourceTestServiceTests
{
    [Test]
    public async Task IfExists_Find_ReturnsData()
    {
        // Arrange
        var service = new LocalSourceTestService();
        var test = service.Items.ElementAt(0);

        // Act
        var result = await service.FindAsync(test.ReferenceNumber);

        // Assert
        result.Should().BeEquivalentTo(test);
    }

    [Test]
    public async Task IfNotExists_Find_ReturnsNull()
    {
        // Arrange
        var service = new LocalSourceTestService();

        // Act
        var result = await service.FindAsync(0);

        // Assert
        result.Should().BeNull();
    }
}
