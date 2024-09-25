using AirWeb.Domain.Comments;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.SampleData;

namespace LocalRepositoryTests.Fces;

public class AddCommentTests
{
    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetFceRepository();

        var fceId = FceData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(fceId, newComment);
        var fceInRepo = await repository.GetAsync(fceId);

        // Assert
        fceInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
