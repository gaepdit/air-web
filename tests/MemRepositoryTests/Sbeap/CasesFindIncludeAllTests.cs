using AirWeb.MemRepository.SbeapRepositories;

namespace MemRepositoryTests.Sbeap;

public class CasesFindIncludeAllTests
{
    private CaseworkMemRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = new CaseworkMemRepository(new ActionItemMemRepository());

    [TearDown]
    public async Task TearDown() => await _repository.DisposeAsync();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindIncludeAllAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
