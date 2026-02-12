using AirWeb.AppServices.DtoInterfaces;
using AirWeb.Core.BaseEntities;

namespace AirWeb.AppServices.Compliance.Fces;

public interface IFceBasicViewDto : IHasOwner, IIsDeleted, IDeleteComments
{
    public int Id { get; }
    public string FacilityId { get; }
    public int Year { get; }
}
