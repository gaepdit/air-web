using AirWeb.Domain.Comments;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.SampleData;

namespace LocalRepositoryTests.ComplianceMonitoring;

public class AddCommentTests
{
    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetWorkEntryRepository();

        var id = ComplianceWorkData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(id, newComment);
        var itemInRepo = await repository.GetAsync(id);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
