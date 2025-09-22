using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace EfRepositoryTests.EnforcementActions;

[TestFixture]
public class GetEnforcementActionTypeTests
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
        var existingAction = EnforcementActionData.GetData.First();
        var expected = existingAction.ActionType;

        // Act
        var results = await _repository.GetEnforcementActionType(existingAction.Id);

        // Assert
        results.Should().Be(expected);
    }

    [Test]
    public async Task GetEnforcementActionType_WhenIdDoesNotExist_ReturnNull()
    {
        // Arrange
        var nonExistingActionId = Guid.NewGuid();

        // Act
        var results = await _repository.GetEnforcementActionType(nonExistingActionId);

        // Assert
        results.Should().BeNull();
    }
}
