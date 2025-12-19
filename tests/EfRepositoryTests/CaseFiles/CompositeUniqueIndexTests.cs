using AirWeb.Domain.EnforcementEntities.CaseFiles;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.CaseFiles;

public class CompositeUniqueIndexTests
{
    private readonly FacilityId _facilityId = (FacilityId)"00199999";

    [Test]
    public async Task Insert_DifferentFacilitiesSameActionNumber_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();

        var caseFile1 = new CaseFile(null, (FacilityId)"00199998", null) { ActionNumber = 1 };
        var caseFile2 = new CaseFile(null, (FacilityId)"00199999", null) { ActionNumber = 1 };

        // Act
        await repository.InsertAsync(caseFile1);
        await repository.InsertAsync(caseFile2);

        // Assert - both should be saved successfully
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<CaseFile>().CountAsync(f => f.ActionNumber == 1)).Should().Be(2);
    }

    [Test]
    public async Task Insert_SameFacilitySameActionNumber_Fails()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();

        var caseFile1 = new CaseFile(null, _facilityId, null) { ActionNumber = 1 };
        var caseFile2 = new CaseFile(null, _facilityId, null) { ActionNumber = 1 };

        // Act
        await repository.InsertAsync(caseFile1);
        var action = async () => await repository.InsertAsync(caseFile2);

        // Assert - should throw DbUpdateException due to unique constraint violation
        await action.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task Insert_SameFacilityDifferentActionNumber_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();
        await repositoryHelper.ClearTableAsync<CaseFile, int>();

        var caseFile1 = new CaseFile(null, _facilityId, null) { ActionNumber = 1 };
        var caseFile2 = new CaseFile(null, _facilityId, null) { ActionNumber = 2 };

        // Act
        await repository.InsertAsync(caseFile1);
        await repository.InsertAsync(caseFile2);

        // Assert - both should be saved successfully
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<CaseFile>()
            .CountAsync(f => f.FacilityId == _facilityId.ToString())).Should().Be(2);
    }
}
