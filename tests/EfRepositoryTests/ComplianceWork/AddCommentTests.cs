using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.SampleData;

namespace EfRepositoryTests.ComplianceWork;

public class AddCommentTests
{
    [Test]
    [Platform("Win")]
    public async Task AddComment_InSqlServer_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetWorkEntryRepository();

        var id = WorkEntryData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(id, newComment);
        repositoryHelper.ClearChangeTracker();
        var itemInRepo = await repository.GetAsync(id, IWorkEntryRepository.IncludeComments);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddComment_InSqlite_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetWorkEntryRepository();

        var id = WorkEntryData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(id, newComment);
        repositoryHelper.ClearChangeTracker();
        var itemInRepo = await repository.GetAsync(id, IWorkEntryRepository.IncludeComments);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
