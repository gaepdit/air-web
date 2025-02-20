using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.CommonInterfaces;

public interface IDeletable : IIsDeleted
{
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
}
