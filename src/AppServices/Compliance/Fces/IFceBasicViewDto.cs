using AirWeb.AppServices.CommonInterfaces;

namespace AirWeb.AppServices.Compliance.Fces;

public interface IFceBasicViewDto : IHasOwnerAndDeletable
{
    public int Id { get; }
    public string FacilityId { get; }
    public int Year { get; }
}
