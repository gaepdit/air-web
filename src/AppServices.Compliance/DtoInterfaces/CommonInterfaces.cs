using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Domain.Core.BaseEntities;

namespace AirWeb.AppServices.Compliance.DtoInterfaces;

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
