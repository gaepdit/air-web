using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.EfRepository.ComplianceRepositories;
using AirWeb.TestData.Enforcement;
using GaEpd.AppLibrary.Domain.Repositories;

namespace EfRepositoryTests.CaseFiles;

[TestFixture]
public class GetAirProgramsTest
{
    private CaseFileRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetCaseFileRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenAirProgramsExist_Return()
    {
        // Arrange
        var caseFile = CaseFileData.GetData.First(e => e.AirPrograms.Count > 0);
        var expected = caseFile.AirPrograms;

        // Act
        var results = await _repository.GetAirProgramsAsync(caseFile.Id);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GivenNoAirPrograms_ReturnEmpty()
    {
        // Arrange
        var caseFile = CaseFileData.GetData.First(e => e.AirPrograms.Count == 0);

        // Act
        var results = await _repository.GetAirProgramsAsync(caseFile.Id);

        // Assert
        results.Should().BeNullOrEmpty();
    }

    // Given an ID that doesn't exist, throw Exception
    [Test]
    public async Task GivenInvalidId_ThrowsException()
    {
        //Arrange
        const int invalidId = -999;

        //Act
        Func<Task> act = async () => await _repository.GetAirProgramsAsync(invalidId);

        //Assert
        await act.Should().ThrowAsync<EntityNotFoundException<CaseFile>>();
    }
}
