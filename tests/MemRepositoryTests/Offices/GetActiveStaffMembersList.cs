using AirWeb.MemRepository.CommonRepositories;

namespace MemRepositoryTests.Offices;

public class GetActiveStaffMembersList
{
    private OfficeMemRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        // Arrange
        var officeId = _repository.Staff.Users.First(e => e.Active).Office?.Id;
        var expected = _repository.Staff.Users
            .Where(e => e.Office != null && e.Office.Id.Equals(officeId));

        // Act
        var result = await _repository.GetStaffMembersListAsync(officeId!.Value, true);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_ReturnsEmptyList()
    {
        var id = Guid.Empty;
        var result = await _repository.GetStaffMembersListAsync(id, false);
        result.Should().BeEmpty();
    }
}
