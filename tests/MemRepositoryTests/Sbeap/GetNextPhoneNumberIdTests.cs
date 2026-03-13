using AirWeb.MemRepository.SbeapRepositories;

namespace MemRepositoryTests.Sbeap;

public class GetNextPhoneNumberIdTests
{
    private ContactMemRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = new ContactMemRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();


    [Test]
    public void GivenLocalRepository_ReturnsNextIdNumber()
    {
        var maxId = _repository.Items
            .SelectMany(e => e.PhoneNumbers)
            .Max(e => e.Id);
        _repository.GetNextPhoneNumberId().Should().Be(maxId + 1);
    }
}
