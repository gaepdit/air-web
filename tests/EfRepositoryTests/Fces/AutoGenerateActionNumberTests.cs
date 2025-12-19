using AirWeb.Domain.ComplianceEntities.Fces;
using IaipDataService.Facilities;
using Microsoft.EntityFrameworkCore;

namespace EfRepositoryTests.Fces;

public class AutoGenerateActionNumberTests
{
    [Test]
    public async Task Insert_WithoutActionNumber_GeneratesSequentialNumber()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var facilityId = (FacilityId)"00100001";
        var fce1 = new Fce(null, facilityId, 2020, null);
        var fce2 = new Fce(null, facilityId, 2021, null);
        var fce3 = new Fce(null, facilityId, 2022, null);

        // Act
        await repository.InsertAsync(fce1);
        await repository.InsertAsync(fce2);
        await repository.InsertAsync(fce3);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedFces = await repositoryHelper.Context.Set<Fce>()
            .Where(f => f.FacilityId == facilityId.ToString())
            .OrderBy(f => f.ActionNumber)
            .ToListAsync();

        savedFces.Should().HaveCount(3);
        savedFces[0].ActionNumber.Should().Be(1);
        savedFces[1].ActionNumber.Should().Be(2);
        savedFces[2].ActionNumber.Should().Be(3);
    }

    [Test]
    public async Task Insert_DifferentFacilities_StartsAtOneForEach()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var facilityA = (FacilityId)"00100001";
        var facilityB = (FacilityId)"00100002";

        var fceA1 = new Fce(null, facilityA, 2020, null);
        var fceB1 = new Fce(null, facilityB, 2020, null);
        var fceA2 = new Fce(null, facilityA, 2021, null);

        // Act
        await repository.InsertAsync(fceA1);
        await repository.InsertAsync(fceB1);
        await repository.InsertAsync(fceA2);

        // Assert
        repositoryHelper.ClearChangeTracker();
        
        var facilityAFces = await repositoryHelper.Context.Set<Fce>()
            .Where(f => f.FacilityId == facilityA.ToString())
            .OrderBy(f => f.ActionNumber)
            .ToListAsync();

        var facilityBFces = await repositoryHelper.Context.Set<Fce>()
            .Where(f => f.FacilityId == facilityB.ToString())
            .ToListAsync();

        facilityAFces.Should().HaveCount(2);
        facilityAFces[0].ActionNumber.Should().Be(1);
        facilityAFces[1].ActionNumber.Should().Be(2);

        facilityBFces.Should().HaveCount(1);
        facilityBFces[0].ActionNumber.Should().Be(1);
    }

    [Test]
    public async Task Insert_WithExistingActionNumber_PreservesIt()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var facilityId = (FacilityId)"00100001";
        var fce1 = new Fce(null, facilityId, 2020, null) { ActionNumber = 100 };

        // Act
        await repository.InsertAsync(fce1);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedFce = await repositoryHelper.Context.Set<Fce>()
            .FirstOrDefaultAsync(f => f.FacilityId == facilityId.ToString());

        savedFce.Should().NotBeNull();
        savedFce!.ActionNumber.Should().Be(100);
    }

    [Test]
    public async Task Insert_AfterManualActionNumber_ContinuesSequence()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await repositoryHelper.ClearTableAsync<Fce, int>();

        var facilityId = (FacilityId)"00100001";
        var fce1 = new Fce(null, facilityId, 2020, null) { ActionNumber = 5 };
        var fce2 = new Fce(null, facilityId, 2021, null);

        // Act
        await repository.InsertAsync(fce1);
        await repository.InsertAsync(fce2);

        // Assert
        repositoryHelper.ClearChangeTracker();
        var savedFces = await repositoryHelper.Context.Set<Fce>()
            .Where(f => f.FacilityId == facilityId.ToString())
            .OrderBy(f => f.ActionNumber)
            .ToListAsync();

        savedFces.Should().HaveCount(2);
        savedFces[0].ActionNumber.Should().Be(5);
        savedFces[1].ActionNumber.Should().Be(6);
    }
}
