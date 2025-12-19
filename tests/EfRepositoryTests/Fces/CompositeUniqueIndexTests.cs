using AirWeb.Domain.ComplianceEntities.Fces;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.Fces;

public class CompositeUniqueIndexTests
{
    private readonly FacilityId _facilityId = (FacilityId)"00199999";

    [Test]
    public async Task Insert_DifferentFacilitiesSameActionNumber_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var fce1 = new Fce(null, (FacilityId)"00199998", 2020, null) { ActionNumber = 1 };
        var fce2 = new Fce(null, (FacilityId)"00199999", 2020, null) { ActionNumber = 1 };

        // Act
        await repository.InsertAsync(fce1);
        await repository.InsertAsync(fce2);

        // Assert - both should be saved successfully
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<Fce>().CountAsync(f => f.ActionNumber == 1)).Should().Be(2);
    }

    [Test]
    public async Task Insert_SameFacilitySameActionNumber_Fails()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var fce1 = new Fce(null, _facilityId, 2020, null) { ActionNumber = 1 };
        var fce2 = new Fce(null, _facilityId, 2021, null) { ActionNumber = 1 };

        // Act
        await repository.InsertAsync(fce1);
        var action = async () => await repository.InsertAsync(fce2);

        // Assert - should throw DbUpdateException due to unique constraint violation
        await action.Should().ThrowAsync<DbUpdateException>();
    }

    [Test]
    public async Task Insert_SameFacilityDifferentActionNumber_Succeeds()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var fce1 = new Fce(null, _facilityId, 2020, null) { ActionNumber = 1 };
        var fce2 = new Fce(null, _facilityId, 2021, null) { ActionNumber = 2 };

        // Act
        await repository.InsertAsync(fce1);
        await repository.InsertAsync(fce2);

        // Assert - both should be saved successfully
        repositoryHelper.ClearChangeTracker();
        (await repositoryHelper.Context.Set<Fce>()
            .CountAsync(f => f.FacilityId == _facilityId.ToString())).Should().Be(2);
    }
}
