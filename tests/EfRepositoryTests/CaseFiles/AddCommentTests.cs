using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.TestData.Enforcement;
using AirWeb.TestData.SampleData;

namespace EfRepositoryTests.CaseFiles;

public class AddCommentTests
{
    [Test]
    [Platform("Win")]
    public async Task AddComment_InSqlServer_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetCaseFileRepository();

        var id = CaseFileData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(id, newComment);
        repositoryHelper.ClearChangeTracker();
        var itemInRepo = await repository.GetAsync(id, IFceRepository.IncludeComments);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddComment_InSqlite_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetCaseFileRepository();

        var id = CaseFileData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(id, newComment);
        repositoryHelper.ClearChangeTracker();
        var itemInRepo = await repository.GetAsync(id, IFceRepository.IncludeComments);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
