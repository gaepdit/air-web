using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Core.Entities;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;
using AirWeb.TestData.SampleData;

namespace LocalRepositoryTests.Comments;

public class AddCommentTests
{
    [Test]
    public async Task AddFceComment_AddsComment()
    {
        // Arrange
        await using var repository = new LocalFceCommentRepository();
        await using var fceRepository = RepositoryHelper.GetFceRepository();

        var id = FceData.GetData.First().Id;
        var newComment = new FceComment(new Comment(SampleText.ValidName, null), id);

        // Act
        await repository.AddCommentAsync(id, newComment);
        var itemInRepo = await fceRepository.GetAsync(id);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddComplianceWorkComment_AddsComment()
    {
        // Arrange
        await using var repository = new LocalComplianceWorkCommentRepository();
        await using var complianceWorkRepository = RepositoryHelper.GetComplianceWorkRepository();

        var id = ComplianceWorkData.GetData.First().Id;
        var newComment = new ComplianceWorkComment(new Comment(SampleText.ValidName, null), id);

        // Act
        await repository.AddCommentAsync(id, newComment);
        var itemInRepo = await complianceWorkRepository.GetAsync(id);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }

    [Test]
    public async Task AddCaseFileComment_AddsComment()
    {
        // Arrange
        await using var repository = new LocalCaseFileCommentRepository();
        await using var caseFileRepository = RepositoryHelper.GetCaseFileRepository();

        var id = CaseFileData.GetData.First().Id;
        var newComment = new CaseFileComment(new Comment(SampleText.ValidName, null), id);

        // Act
        await repository.AddCommentAsync(id, newComment);
        var itemInRepo = await caseFileRepository.GetAsync(id);

        // Assert
        itemInRepo.Comments.OrderByDescending(comment => comment.CommentedAt).First()
            .Should().BeEquivalentTo(newComment);
    }
}
