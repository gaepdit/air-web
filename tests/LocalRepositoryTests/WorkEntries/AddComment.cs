using AirWeb.Domain.ValueObjects;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.WorkEntries;

public class AddComment
{
    private LocalWorkEntryRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetWorkEntryRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        var entry = WorkEntryData.GetData.First();
        var comment = Comment.CreateComment("abc", null);
        await _repository.AddCommentAsync(entry.Id, comment);

        // Act
        var entryInRepo = await _repository.GetAsync(entry.Id);

        // Assert
        entryInRepo.Comments[^1].Should().BeEquivalentTo(comment);
    }
}
