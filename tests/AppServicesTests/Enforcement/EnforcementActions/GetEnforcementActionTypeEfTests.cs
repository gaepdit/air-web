using AirWeb.AppServices.Core.AppNotifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Enforcement;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.Enforcement;
using EfRepositoryTests;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Enforcement.EnforcementActions;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class GetEnforcementActionTypeEfTests
{
    private EnforcementActionService _sut;
    private EnforcementActionRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _repository = RepositoryHelper.CreateRepositoryHelper().GetEnforcementActionRepository();
        _sut = new EnforcementActionService(Substitute.For<IEnforcementActionManager>(), _repository,
            Substitute.For<ICaseFileRepository>(), Substitute.For<ICaseFileManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), Substitute.For<ILogger<EnforcementActionService>>(),
            Substitute.For<IAppNotificationService>());
    }

    [TearDown]
    public void TearDown()
    {
        _sut.Dispose();
        _repository.Dispose();
    }

    [Test]
    public async Task EF_GetEnforcementActionType_WhenIdExists_ReturnActionType()
    {
        // Arrange
        var existingAction = EnforcementActionData.GetData.First();
        var expected = existingAction.ActionType;

        // Act
        var results = await _sut.GetEnforcementActionType(existingAction.Id);

        // Assert
        results.Should().Be(expected);
    }

    [Test]
    public async Task EF_GetEnforcementActionType_WhenIdDoesNotExist_ReturnNull()
    {
        // Arrange
        var nonExistingActionId = Guid.NewGuid();

        // Act
        var results = await _sut.GetEnforcementActionType(nonExistingActionId);

        // Assert
        results.Should().BeNull();
    }
}
