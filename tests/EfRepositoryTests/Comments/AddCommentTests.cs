using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Core.Entities;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.SampleData;

namespace EfRepositoryTests.Comments;

public class AddCommentTests
{
    [Test]
    [Platform("Win")]
    public async Task AddComment_InSqlServer_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var fceRepository = repositoryHelper.GetFceRepository();
        await using var commentRepository = repositoryHelper.GetFceCommentRepository();

        var id = FceData.GetData.First().Id;
        var newComment = new FceComment(new Comment(SampleText.ValidName, null), id);

        // Act
        await commentRepository.AddCommentAsync(id, newComment);
        repositoryHelper.ClearChangeTracker();
        var itemInRepo = await fceRepository.GetAsync(id, IFceRepository.IncludeComments);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddComment_InSqlite_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();
        await using var commentRepository = repositoryHelper.GetFceCommentRepository();

        var id = FceData.GetData.First().Id;
        var newComment = new FceComment(new Comment(SampleText.ValidName, null), id);

        // Act
        await commentRepository.AddCommentAsync(id, newComment);
        repositoryHelper.ClearChangeTracker();
        var itemInRepo = await repository.GetAsync(id, IFceRepository.IncludeComments);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
