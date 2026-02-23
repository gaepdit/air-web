using AirWeb.EfRepository.CommonRepositories;
using AirWeb.TestData.Identity;

namespace EfRepositoryTests.Offices;

public class GetActiveStaffMembersList
{
    private OfficeRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        var item = UserData.Users.First(e => e.Active).Office?.Id;
        var result = await _repository.GetStaffMembersListAsync(item!.Value, true);
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_ReturnsEmptyList()
    {
        var id = Guid.Empty;
        var result = await _repository.GetStaffMembersListAsync(id, false);
        result.Should().BeEmpty();
    }
}
