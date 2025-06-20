using AirWeb.AppServices.Staff.Dto;
using AirWeb.Domain.BaseEntities.Interfaces;

namespace AirWeb.AppServices.DtoInterfaces;

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

public interface IResponseRequested
{
    public bool ResponseRequested { get; init; }
}
