using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.Fces;

public class AddComment
{
    [Test]
    public async Task AddComment_InSqlServer_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetFceRepository();

        var fceId = FceData.GetData.First().Id;
        var newComment = Comment.CreateComment("abc", null);

        // Act
        await repository.AddCommentAsync(fceId, newComment);
        repositoryHelper.ClearChangeTracker();
        var fceInRepo = await repository.GetAsync(fceId);

        // Assert
        fceInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddComment_InSqlite_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetFceRepository();

        var fceId = FceData.GetData.First().Id;
        var newComment = Comment.CreateComment("abc", null);

        // Act
        await repository.AddCommentAsync(fceId, newComment);
        repositoryHelper.ClearChangeTracker();
        var fceInRepo = await repository.GetAsync(fceId);

        // Assert
        fceInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
