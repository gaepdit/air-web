using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.CommonInterfaces;

public interface IDeletable
{
    public bool IsDeleted { get; }
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
    public string? DeleteComments { get; }
}
