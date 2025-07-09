using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace LocalRepositoryTests.CaseFiles;

[TestFixture]
public class GetPollutantsTests
{
    private LocalCaseFileRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetCaseFileRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();
    
    [Test]
    public async Task GivenPollutantsExist_ReturnsListOfPollutants()
    {
        // Arrange
        var caseFile = CaseFileData.GetData.First(e => e.PollutantIds.Count > 0);
        var expected = caseFile.GetPollutants();
        
        // Act
        var results =await _repository.GetPollutantsAsync(caseFile.Id);
        
        // Assert
        results.Should().BeEquivalentTo(expected);
    }
    
    // Given no pollutant return empty listj
    // Given and ID that doesn't exist, return ...
}
