using AirWeb.AppServices.Staff.Dto;

namespace AirWeb.AppServices.CommonInterfaces;

public interface IDeletedItem
{
    public string ItemName { get; }
    public string ItemId { get; }
    public bool IsDeleted { get; }
    public StaffViewDto? DeletedBy { get; }
    public DateTimeOffset? DeletedAt { get; }
    public string? DeleteComments { get; }
}
