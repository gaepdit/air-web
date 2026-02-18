using AirWeb.AppServices.Compliance.Enforcement;
using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionQuery;
using AirWeb.AppServices.Core.AppNotifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AirWeb.Domain.Compliance.EnforcementEntities.EnforcementActions;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Compliance.Enforcement.EnforcementActions;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class GetEnforcementActionTypeTests
{
    private EnforcementActionService _sut;
    private IEnforcementActionRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IEnforcementActionRepository>();
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
        var existingActionId = Guid.NewGuid();
        var existingAction = new ActionTypeDto { ActionType = EnforcementActionType.AdministrativeOrder };
        _repository.FindAsync<ActionTypeDto>(existingActionId, Arg.Any<IMapper>(), Arg.Any<CancellationToken>())
            .Returns(existingAction);
        var expected = existingAction.ActionType;

        // Act
        var results = await _sut.GetEnforcementActionType(existingActionId);

        // Assert
        results.Should().Be(expected);
    }

    [Test]
    public async Task EF_GetEnforcementActionType_WhenIdDoesNotExist_ReturnNull()
    {
        // Arrange
        var nonExistingActionId = Guid.NewGuid();
        _repository.FindAsync<ActionTypeDto>(nonExistingActionId, Arg.Any<IMapper>(), Arg.Any<CancellationToken>())
            .Returns((ActionTypeDto?)null);

        // Act
        var results = await _sut.GetEnforcementActionType(nonExistingActionId);

        // Assert
        results.Should().BeNull();
    }
}
