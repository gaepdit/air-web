using AirWeb.Domain.Identity;
using AirWeb.LocalRepository.Identity;
using AirWeb.TestData.Identity;
using System.Diagnostics;

namespace LocalRepositoryTests.Identity;

public class UserRoleStore
{
    private LocalUserStore _store;

    [SetUp]
    public void SetUp() => _store = RepositoryHelper.GetUserStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task AddToRole_AddsRole()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);

        var roleName = UserData.Roles[0].Name;
        Debug.Assert(roleName != null);
        var resultBefore = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        // Act
        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);

        // Assert
        var resultAfter = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);
        using var scope = new AssertionScope();
        resultBefore.Should().BeFalse();
        resultAfter.Should().BeTrue();
    }

    [Test]
    public async Task RemoveFromRole_RemovesRole()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);

        var roleName = UserData.Roles[0].Name;
        Debug.Assert(roleName != null);
        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);
        var resultBefore = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        // Act
        await _store.RemoveFromRoleAsync(user, roleName, CancellationToken.None);

        // Arrange
        var resultAfter = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);
        using var scope = new AssertionScope();
        resultBefore.Should().BeTrue();
        resultAfter.Should().BeFalse();
    }

    [Test]
    public async Task GetRoles_ReturnsListOfRoles()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);

        var roleName = UserData.Roles[0].Name;
        Debug.Assert(roleName != null);
        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);

        // Act
        var result = await _store.GetRolesAsync(user, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.Should().ContainSingle();
    }

    [Test]
    public async Task GetRoles_IfNone_ReturnsEmptyList()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);

        // Act
        var result = await _store.GetRolesAsync(user, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task IsInRole_IfSo_ReturnsTrue()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);

        var roleName = UserData.Roles[0].Name;
        Debug.Assert(roleName != null);
        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);

        // Act
        var result = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task IsInRole_IfNot_ReturnsFalse()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);

        var roleName = UserData.Roles[0].Name;
        Debug.Assert(roleName != null);

        // Act
        var result = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task GetUsersInRole_IfSome_ReturnsListOfUsers()
    {
        // Arrange
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };
        UserData.Users.Add(user);
        var roleName = UserData.Roles[0].Name;
        Debug.Assert(roleName != null);
        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);

        // Act
        var result = await _store.GetUsersInRoleAsync(roleName, CancellationToken.None);

        // Assert
        result.Should().ContainEquivalentOf(user,
            options => options.Excluding(e => e.Office));
    }

    [Test]
    public async Task GetUsersInRole_IfNone_ReturnsEmptyList()
    {
        // Act
        var result = await _store.GetUsersInRoleAsync("None", CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
