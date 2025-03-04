using AirWeb.Domain.Identity;
using AirWeb.Domain.NamedEntities.Offices;
using AirWeb.LocalRepository.Identity;
using AirWeb.TestData.Identity;
using AirWeb.TestData.SampleData;
using GaEpd.AppLibrary.Domain.Repositories;
using System.Diagnostics;

namespace LocalRepositoryTests.Identity;

public class UserStore
{
    private LocalUserStore _store = null!;

    [SetUp]
    public void SetUp() => _store = RepositoryHelper.GetUserStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task GetUserId_ReturnsId()
    {
        var user = UserData.GetUsers.First();
        var result = await _store.GetUserIdAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.Id);
    }

    [Test]
    public async Task GetUserName_ReturnsUserName()
    {
        var user = _store.UserStore.First();
        var result = await _store.GetUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.UserName);
    }

    [Test]
    public async Task GetNormalizedUserName_ReturnsNormalizedUserName()
    {
        var user = _store.UserStore.First();
        var result = await _store.GetNormalizedUserNameAsync(user, CancellationToken.None);
        result.Should().BeEquivalentTo(user.NormalizedUserName);
    }

    [Test]
    public async Task Update_WhenItemIsValid_UpdatesItem()
    {
        var store = RepositoryHelper.GetUserStore();
        var user = store.UserStore.First();
        user.PhoneNumber = "1";
        user.Office = new Office(Guid.NewGuid(), SampleText.ValidName);

        var result = await store.UpdateAsync(user, CancellationToken.None);
        var updatedUser = await store.FindByIdAsync(user.Id, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Succeeded.Should().BeTrue();
        updatedUser.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Update_WhenItemDoesNotExist_Throws()
    {
        var user = new ApplicationUser { Id = Guid.Empty.ToString() };
        var action = async () => await _store.UpdateAsync(user, CancellationToken.None);
        await action.Should().ThrowAsync<EntityNotFoundException<ApplicationUser>>();
    }

    [Test]
    public async Task FindById_ReturnsUser()
    {
        var user = _store.UserStore.First();
        var result = await _store.FindByIdAsync(user.Id, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task FindByName_ReturnsUser()
    {
        var user = _store.UserStore.First();
        Debug.Assert(user.NormalizedUserName != null);
        var result = await _store.FindByNameAsync(user.NormalizedUserName, CancellationToken.None);
        result.Should().BeEquivalentTo(user);
    }
}
