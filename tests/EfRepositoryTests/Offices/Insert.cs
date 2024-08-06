using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.TestData.SampleData;

namespace EfRepositoryTests.Offices;

public class Insert
{
    [Test]
    public async Task InsertItem_InSqlServer_IncreasesCount()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetOfficeRepository();

        var initialCount = repositoryHelper.Context.Set<Office>().Count();
        var entity = new Office(Guid.NewGuid(), SampleText.ValidName);

        // Act
        await repository.InsertAsync(entity);

        // Assert
        repositoryHelper.ClearChangeTracker();
        repositoryHelper.Context.Set<Office>().Count().Should().Be(initialCount + 1);
    }

    [Test]
    public async Task InsertItem_InSqlite_IncreasesCount()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetOfficeRepository();

        var initialCount = repositoryHelper.Context.Set<Office>().Count();
        var entity = new Office(Guid.NewGuid(), SampleText.ValidName);

        // Act
        await repository.InsertAsync(entity);

        // Assert
        repositoryHelper.ClearChangeTracker();
        repositoryHelper.Context.Set<Office>().Count().Should().Be(initialCount + 1);
    }
}
