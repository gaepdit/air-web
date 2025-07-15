using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;
using Microsoft.IdentityModel.Tokens;

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
        var results = await _repository.GetPollutantsAsync(caseFile.Id);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    // Given no pollutant return empty list
    [Test]
    public async Task GivenNoPollutants_ReturnEmptyList()
    {
        //Arrange
        var caseFile = CaseFileData.GetData.First(e => e.PollutantIds.Count == 0);

        //Act
        var results = await _repository.GetPollutantsAsync(caseFile.Id);

        //Asert
        results.Should().BeEmpty();
    }

    // Given and ID that doesn't exist, return Exception
    [Test]
    public async Task GivenNoID_ThrowsException()
    {
        //Arrange
        var invalidId = -999;

        //Act
        Func<Task> act = async () => await _repository.GetPollutantsAsync(invalidId);

        //Asert
        await act.Should().ThrowAsync<Exception>();
    }
}
