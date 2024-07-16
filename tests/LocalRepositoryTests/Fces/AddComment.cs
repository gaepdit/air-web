using AirWeb.Domain.ValueObjects;
using AirWeb.LocalRepository.Repositories;
using AirWeb.TestData.Entities;

namespace LocalRepositoryTests.Fces;

public class AddComment
{
    private LocalFceRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetFceRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task AddComment_AddsComment()
    {
        // Arrange
        var fce = FceData.GetData.First();
        var comment = Comment.CreateComment("abc", null);
        await _repository.AddCommentAsync(fce.Id, comment);

        // Act
        var fceInRepo = await _repository.GetAsync(fce.Id);

        // Assert
        fceInRepo.Comments[^1].Should().BeEquivalentTo(comment);
    }
}
