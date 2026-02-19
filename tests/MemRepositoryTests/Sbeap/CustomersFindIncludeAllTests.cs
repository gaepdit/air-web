using AirWeb.MemRepository.SbeapRepositories;

namespace MemRepositoryTests.Sbeap;

public class FindIncludeAll
{
    private CustomerMemRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = new CustomerMemRepository(new ContactMemRepository(),
        new CaseworkMemRepository(new ActionItemMemRepository()));

    [TearDown]
    public async Task TearDown() => await _repository.DisposeAsync();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        item.Contacts.RemoveAll(c => c.IsDeleted);

        var result = await _repository.FindIncludeAllAsync(item.Id, true);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty, true);
        result.Should().BeNull();
    }
}
