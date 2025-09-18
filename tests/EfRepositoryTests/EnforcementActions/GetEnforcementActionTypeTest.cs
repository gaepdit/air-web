using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace EfRepositoryTests.EnforcementActions;

[TestFixture]
public class GetEnforcementActionTypeTest
{
    private EnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementActionRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GetEnforcementActionType_WhenIdExist_ReturnActionType()
    {
        // Arrange
        var existingEAT = EnforcementActionData.GetData.First();
        var expected = existingEAT.ActionType;

        // Act
        var results = await _repository.GetEnforcementActionType(existingEAT.Id);

        // Assert
        results.Should().Be(expected);
    }

    [Test]
    public async Task GetEnforcementActionType_WhenIdDoesNotExist_ReturnNull()
    {
        // Arrange
        var nonExistingEAT = Guid.NewGuid();

        // Act
        var results = await _repository.GetEnforcementActionType(nonExistingEAT);

        // Assert
        results.Should().BeNull();
    }
}
