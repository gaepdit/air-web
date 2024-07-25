using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;
using AirWeb.TestData.SampleData;

namespace LocalRepositoryTests.WorkEntries;

public class AddCommentTests
{
    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();

        var entryId = WorkEntryData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(entryId, newComment);
        var entryInRepo = await repository.GetAsync(entryId);

        // Assert
        entryInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
