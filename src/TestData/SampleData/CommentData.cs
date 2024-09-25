using AirWeb.Domain.Comments;
using AirWeb.TestData.Identity;

namespace AirWeb.TestData.SampleData;

public static class CommentData
{
    public static List<Comment> GetRandomCommentsList(int minValue = 0, int maxValue = 4)
    {
        var commentList = new List<Comment>();
        var commentCount = new Random().Next(maxValue - minValue) + minValue;
        for (var i = 0; i < commentCount; i++)
        {
            commentList.Add(new Comment
            {
                Id = Guid.NewGuid(),
                Text = SampleText.GetRandomText(SampleText.TextLength.Paragraph),
                CommentBy = UserData.GetUsers.ElementAt(i),
                CommentedAt = DateTimeOffset.Now.AddDays(i - commentCount),
            });
        }

        return commentList;
    }
}
