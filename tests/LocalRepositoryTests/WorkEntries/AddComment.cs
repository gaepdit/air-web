using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.WorkEntries;

public class AddComment
{
    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();

        var entryId = WorkEntryData.GetData.First().Id;
        var newComment = Comment.CreateComment("abc", null);

        // Act
        await repository.AddCommentAsync(entryId, newComment);
        var entryInRepo = await repository.GetAsync(entryId);

        // Assert
        entryInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}