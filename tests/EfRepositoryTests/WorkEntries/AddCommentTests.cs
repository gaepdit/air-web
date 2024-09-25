using AirWeb.Domain.Comments;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.SampleData;

namespace EfRepositoryTests.WorkEntries;

public class AddCommentTests
{
    [Test]
    public async Task AddComment_InSqlServer_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        await using var repository = repositoryHelper.GetWorkEntryRepository();

        var entryId = WorkEntryData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(entryId, newComment);
        repositoryHelper.ClearChangeTracker();
        var entryInRepo = await repository.GetAsync(entryId, IWorkEntryRepository.IncludeComments);

        // Assert
        entryInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddComment_InSqlite_AddsComment()
    {
        // Arrange
        await using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
        await using var repository = repositoryHelper.GetWorkEntryRepository();

        var entryId = WorkEntryData.GetData.First().Id;
        var newComment = Comment.CreateComment(SampleText.ValidName, null);

        // Act
        await repository.AddCommentAsync(entryId, newComment);
        repositoryHelper.ClearChangeTracker();
        var entryInRepo = await repository.GetAsync(entryId, IWorkEntryRepository.IncludeComments);

        // Assert
        entryInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
