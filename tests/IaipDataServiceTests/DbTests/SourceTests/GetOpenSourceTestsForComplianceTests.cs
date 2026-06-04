using IaipDataService.SourceTests;

namespace IaipDataServiceTests.DbTests.SourceTests;

public class GetOpenSourceTestsForComplianceTests
{
    private IaipSourceTestService _sut;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _sut = new IaipSourceTestService(Config.DbConnectionFactory!);
    }

    [Test]
    public async Task ReturnsList()
    {
        // Act
        var result = await _sut.GetOpenSourceTestsForComplianceAsync(assignmentUser: null, assignmentOffice: null,
            skip: 0, take: 1);

        // Assert
        using var scope = new AssertionScope();
        result.Item1.Should().ContainSingle();
        result.Item2.Should().BePositive();
    }
}
