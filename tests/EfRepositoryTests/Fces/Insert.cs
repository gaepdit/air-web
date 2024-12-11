using AirWeb.Domain.ComplianceEntities.Fces;
using IaipDataService.Facilities;

namespace EfRepositoryTests.Fces;

public class Insert
{
    private readonly FacilityId _facilityId = (FacilityId)"001-00001";

    [Test]
    public async Task InsertItem_InSqlServer_IncreasesCount()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetFceRepository();

        var initialCount = repositoryHelper.Context.Set<Fce>().Count();
        var entity = new Fce(null, _facilityId, 2000, null);

        // Act
        await repository.InsertAsync(entity);

        // Assert
        repositoryHelper.ClearChangeTracker();
        repositoryHelper.Context.Set<Fce>().Count().Should().Be(initialCount + 1);
    }

    [Test]
    public async Task InsertItem_InSqlite_IncreasesCount()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();

        var initialCount = repositoryHelper.Context.Set<Fce>().Count();
        var entity = new Fce(null, _facilityId, 2000, null);

        // Act
        await repository.InsertAsync(entity);

        // Assert
        repositoryHelper.ClearChangeTracker();
        repositoryHelper.Context.Set<Fce>().Count().Should().Be(initialCount + 1);
    }
}
