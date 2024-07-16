using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Entities;

namespace EfRepositoryTests.Fces;

public class AddComment
{
    private IFceRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetFceRepository();

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
