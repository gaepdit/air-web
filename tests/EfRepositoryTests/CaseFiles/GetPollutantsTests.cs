using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace EfRepositoryTests.CaseFiles;

[TestFixture]
public class GetPollutantsTests
{
    private CaseFileRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetCaseFileRepository();

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

    // Given no pollutant, return empty list
    [Test]
    public async Task GivenNoPollutants_ReturnEmptyList()
    {
        //Arrange
        var caseFile = CaseFileData.GetData.First(e => e.PollutantIds.Count == 0);

        //Act
        var results = await _repository.GetPollutantsAsync(caseFile.Id);

        //Assert
        results.Should().BeEmpty();
    }

    // Given an ID that doesn't exist, throw Exception
    [Test]
    public async Task GivenNoID_ThrowsException()
    {
        //Arrange
        const int invalidId = -999;

        //Act
        Func<Task> act = async () => await _repository.GetPollutantsAsync(invalidId);

        //Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
