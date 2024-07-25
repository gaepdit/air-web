using AirWeb.Domain.Entities.WorkEntries;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.WorkEntries;

public class GetNotificationType
{
    private IWorkEntryRepository _repository = default!;

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
