using AirWeb.AppServices.Compliance.DtoInterfaces;
using AirWeb.Domain.Core.BaseEntities;

namespace AirWeb.AppServices.Compliance.Compliance.Fces;

public interface IFceBasicViewDto : IHasOwner, IIsDeleted, IDeleteComments
{
    public int Id { get; }
    public string FacilityId { get; }
    public int Year { get; }
}
