using AirWeb.Domain.Comments;
using AirWeb.TestData.Enforcement;
using AirWeb.TestData.SampleData;

namespace LocalRepositoryTests.CaseFiles;

public class AddCommentTests
{
    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        await using var repository = RepositoryHelper.GetCaseFileRepository();

        var id = CaseFileData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.GetRandomText(SampleText.TextLength.Paragraph), user: null);

        // Act
        await repository.AddCommentAsync(id, newComment);
        var itemInRepo = await repository.GetAsync(id);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
