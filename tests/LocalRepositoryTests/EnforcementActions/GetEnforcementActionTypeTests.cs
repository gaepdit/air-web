using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;

namespace LocalRepositoryTests.EnforcementActions;

[TestFixture]
public class GetEnforcementActionTypeTests
{
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetEnforcementActionRepository();

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
