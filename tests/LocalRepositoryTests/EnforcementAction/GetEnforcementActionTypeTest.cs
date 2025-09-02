using AirWeb.Domain.BaseEntities.Interfaces;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;


namespace LocalRepositoryTests.EnforcementAction;

[TestFixture]
public class GetEnforcementActionTypeTest
{
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetEnforcementActionRepository();
    
    [TearDown]
    public void TearDown() => _repository.Dispose();
    
    [Test]
    public async Task GetEnforcementActionType_WhenIdExist()
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
    public async Task GetEnforcementActionType_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistingEAT = Guid.NewGuid();

        // Act
        var results = await _repository.GetEnforcementActionType(nonExistingEAT);

        // Assert
        results.Should().BeNull();
    }
}
    
