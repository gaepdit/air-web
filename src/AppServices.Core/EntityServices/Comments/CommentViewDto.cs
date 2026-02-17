using AirWeb.AppServices.Core.EntityServices.Staff.Dto;

namespace AirWeb.AppServices.Core.EntityServices.Comments;

public record CommentViewDto
{
    public Guid Id { get; init; }
    public required string Text { get; init; }
    public required StaffViewDto CommentBy { get; init; }
    public DateTimeOffset CommentedAt { get; init; } = DateTimeOffset.Now;
}
