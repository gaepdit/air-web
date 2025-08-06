using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace EfRepositoryTests.CaseFiles;
[TestFixture]
public class GetAirProgramsTest
{
    private CaseFileRepository _repository = null;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetCaseFileRepository();

    [TearDown]
    public void TearDown() => _repository?.Dispose();

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

}
