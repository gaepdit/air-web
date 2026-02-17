using AirWeb.Domain.Core.Entities;
using AirWeb.TestData.Identity;

namespace AirWeb.TestData.SampleData;

public static class CommentData
{
    public static List<Comment> GetRandomCommentsList(int minValue = 0, int maxValue = 4)
    {
        var commentList = new List<Comment>();
        var commentCount = Random.Shared.Next(maxValue - minValue) + minValue;
        for (var i = 0; i < commentCount; i++)
        {
            commentList.Add(
                new Comment(SampleText.GetRandomText(SampleText.TextLength.Paragraph), UserData.GetRandomUser())
                    { CommentedAt = DateTimeOffset.Now.AddDays(i - commentCount) });
        }

        return commentList;
    }
}
