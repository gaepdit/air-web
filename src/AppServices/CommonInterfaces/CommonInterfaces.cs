using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.CommonInterfaces;

public interface IDeletable : IIsDeleted
{
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
}

public interface IDeleteComments
{
    public string? DeleteComments { get; }
}

public interface IHasOwner
{
    public string OwnerId { get; }
}

public interface IIsClosed
{
    public bool IsClosed { get; }
}

public interface IIsDeleted
{
    public bool IsDeleted { get; }
}

public interface IResponseRequested
{
    public bool ResponseRequested { get; init; }
}
