using System.Diagnostics.CodeAnalysis;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;
using Microsoft.IdentityModel.Tokens;

namespace LocalRepositoryTests.CaseFiles;

[TestFixture]
public class GetAirProgramsTest
{
    private LocalCaseFileRepository _repository = null;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetCaseFileRepository();

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
