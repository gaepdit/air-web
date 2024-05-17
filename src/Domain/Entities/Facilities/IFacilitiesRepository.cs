namespace AirWeb.Domain.Entities.Facilities;

public interface IFacilitiesRepository
{
    Task<bool> FacilityExistsAsync(FacilityId id);
    Task<Facility?> FindFacilityAsync(FacilityId id);
}
