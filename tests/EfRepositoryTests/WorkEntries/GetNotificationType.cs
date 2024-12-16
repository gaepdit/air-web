using AirWeb.EfRepository.Repositories;
using AirWeb.TestData.NamedEntities;

namespace EfRepositoryTests.WorkEntries;

public class GetNotificationType
{
    private WorkEntryRepository _repository = null!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task GivenExistingItem_ReturnsTrue()
    {
        // Arrange
        var notificationType = NotificationTypeData.GetData[0];

        // Act
        var result = await _repository.GetNotificationTypeAsync(notificationType.Id);

        // Assert
        result.Should().BeEquivalentTo(notificationType);
    }

    [Test]
    public async Task GivenNonexistentId_Throws()
    {
        // Act
        var func = async () => await _repository.GetNotificationTypeAsync(Guid.Empty);

        // Assert
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}
