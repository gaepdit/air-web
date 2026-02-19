using IaipDataService.SourceTests;

namespace IaipDataServiceTests.DbTests.SourceTests;

public class GetOpenSourceTestsForCompliance
{
    private IaipSourceTestService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        using var cache = Substitute.For<IMemoryCache>();
        var logger = Substitute.For<ILogger<IaipSourceTestService>>();
        _sut = new IaipSourceTestService(Config.DbConnectionFactory!, cache, logger);
    }

    [Test]
    public async Task ReturnsList()
    {
        // Act
        var result = await _sut.GetOpenSourceTestsForComplianceAsync(assignmentEmail: null, skip: 0, take: 1);

        // Assert
        using var scope = new AssertionScope();
        result.Item1.Should().ContainSingle();
        result.Item2.Should().BePositive();
    }
}
