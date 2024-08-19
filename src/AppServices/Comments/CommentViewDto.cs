using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.Comments;

public record CommentViewDto
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public required StaffViewDto CommentBy { get; init; }
    public DateTimeOffset CommentedAt { get; init; } = DateTimeOffset.Now;
}
