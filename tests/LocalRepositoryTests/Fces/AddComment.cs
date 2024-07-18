using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.Fces;

public class AddComment
{
    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetFceRepository();

        var fceId = FceData.GetData.First().Id;
        var newComment = Comment.CreateComment("abc", null);

        // Act
        await repository.AddCommentAsync(fceId, newComment);
        var fceInRepo = await repository.GetAsync(fceId);

        // Assert
        fceInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
