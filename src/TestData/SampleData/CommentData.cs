using AirWeb.Domain.ValueObjects;
using AirWeb.TestData.Identity;

namespace AirWeb.TestData.SampleData;

public static class CommentData
{
    public static List<Comment> GetRandomCommentsList()
    {
        var commentList = new List<Comment>();

        for (var i = 0; i < new Random().Next(4); i++)
        {
            commentList.Add(new Comment
            {
                Id = Guid.NewGuid(),
                Text = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
                CommentBy = UserData.GetUsers.ElementAt(i),
                CommentedAt = DateTimeOffset.Now.AddDays(-i),
            });
        }

        return commentList;
    }
}
