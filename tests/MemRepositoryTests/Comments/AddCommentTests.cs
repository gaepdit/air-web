using AirWeb.Domain.Compliance.Comments;
using AirWeb.Domain.Core.Entities;
using AirWeb.MemRepository.ComplianceRepositories;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Enforcement;
using AirWeb.TestData.SampleData;

namespace MemRepositoryTests.Comments;

public class AddCommentTests
{
    [Test]
    public async Task AddFceComment_AddsComment()
    {
        // Arrange
        await using var fceRepository = RepositoryHelper.GetFceRepository();
        await using var repository = new FceCommentMemRepository(fceRepository);

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
        await using var complianceWorkRepository = RepositoryHelper.GetComplianceWorkRepository();
        await using var repository = new ComplianceWorkCommentMemRepository(complianceWorkRepository);

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
        await using var caseFileRepository = RepositoryHelper.GetCaseFileRepository();
        await using var repository = new CaseFileCommentMemRepository(caseFileRepository);

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
