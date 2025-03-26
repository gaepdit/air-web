﻿using AirWeb.AppServices.Enforcement;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Users;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;
using AirWeb.TestData.SampleData;
using IaipDataService.Facilities;

namespace AppServicesTests.Enforcement.EnforcementActions;

public class CreateEnforcementActionTests
{
    [Test]
    public async Task OnSuccessfulCreate_InsertGetsCalled()
    {
        // Arrange
        const int id = 901;
        var facilityId = (FacilityId)"00100001";

        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Email = SampleText.ValidEmail };
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns(user);

        var caseFile = new CaseFile(id, facilityId, user);
        var caseFileRepositoryMock = Substitute.For<ICaseFileRepository>();
        caseFileRepositoryMock.GetAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(caseFile);

        var enforcementActionRepositoryMock = Substitute.For<IEnforcementActionRepository>();

        var enforcementActionService = new EnforcementActionService(new EnforcementActionManager(),
            enforcementActionRepositoryMock, caseFileRepositoryMock, Substitute.For<ICaseFileManager>(),
            AppServicesTestsSetup.Mapper!, userServiceMock);
        var item = new CreateEnforcementActionDto { ActionType = EnforcementActionType.LetterOfNoncompliance };

        // Act
        await enforcementActionService.CreateAsync(id, item, CancellationToken.None);

        // Assert
        await enforcementActionRepositoryMock.Received(1)
            .InsertAsync(Arg.Any<EnforcementAction>(), token: Arg.Any<CancellationToken>());
    }
}
