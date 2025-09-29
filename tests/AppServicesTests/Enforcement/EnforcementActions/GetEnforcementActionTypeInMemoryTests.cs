using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Enforcement;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Enforcement;
using LocalRepositoryTests;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Enforcement.EnforcementActions;

[TestFixture]
public class GetEnforcementActionTypeInMemoryTests
{
    private EnforcementActionService _sut;
    private LocalEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _repository = RepositoryHelper.GetEnforcementActionRepository();
        _sut = new EnforcementActionService(Substitute.For<IEnforcementActionManager>(), _repository,
            Substitute.For<ICaseFileRepository>(), Substitute.For<ICaseFileManager>(), AppServicesTestsSetup.Mapper!,
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
    public async Task GetEnforcementActionType_WhenIdExists_ReturnActionType()
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
    public async Task GetEnforcementActionType_WhenIdDoesNotExist_ReturnNull()
    {
        // Arrange
        var nonExistingActionId = Guid.NewGuid();

        // Act
        var results = await _sut.GetEnforcementActionType(nonExistingActionId);

        // Assert
        results.Should().BeNull();
    }
}
