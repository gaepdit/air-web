using IaipDataService.SourceTests;

namespace IaipDataServiceTests;

public class LocalSourceTestServiceTests
{
    private LocalSourceTestService _service = default!;

    [SetUp]
    public void SetUp() => _service = new LocalSourceTestService();

    [Test]
    public async Task IfExists_Find_ReturnsData()
    {
        // Arrange
        var test = SourceTestData.GetData[0];

        // Act
        var result = await _service.FindAsync(test.ReferenceNumber);

        // Assert
        result.Should().BeEquivalentTo(test);
    }

    [Test]
    public async Task IfNotExists_Find_ReturnsNull()
    {
        // Act
        var result = await _service.FindAsync(0);

        // Assert
        result.Should().BeNull();
    }
}
