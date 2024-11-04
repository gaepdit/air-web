using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Structs;
using System.Data;

namespace IaipDataService.Facilities;

public sealed class IaipFacilityService(IDbConnectionFactory dbf) : IFacilityService
{
    public async Task<Facility> GetAsync(FacilityId id, CancellationToken token = default)
    {
        var facility = await FindAsync(id);
        if (facility is null) throw new InvalidOperationException("Facility not found.");
        return facility;
    }

    public async Task<Facility?> FindAsync(FacilityId? id)
    {
        if (id is null || !await ExistsAsync(id)) return null;

        using var db = dbf.Create();

        var varMultiTask = db.QueryMultipleAsync("air.GetIaipFacility",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure);

        await using var multi = await varMultiTask;
        var facility = multi.Read<Facility, Address, GeoCoordinates, RegulatoryData, Facility>(
            (facility, facilityAddress, geoCoordinates, regulatoryData) =>
            {
                facility.FacilityAddress = facilityAddress;
                facility.GeoCoordinates = geoCoordinates;
                facility.RegulatoryData = regulatoryData;
                return facility;
            }, splitOn: "FacilityAddressId,GeoCoordinatesId,RegulatoryDataId").Single();

        facility.RegulatoryData!.AirPrograms.AddRange(await multi.ReadAsync<AirProgram>());
        facility.RegulatoryData!.ProgramClassifications.AddRange(await multi.ReadAsync<AirProgramClassifications>());
        facility.RegulatoryData!.Pollutants = (await multi.ReadAsync<(string Key, string Value)>())
            .ToDictionary(x => x.Key, x => x.Value);

        return facility;
    }

    public async Task<bool> ExistsAsync(FacilityId id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.IaipFacilityExists",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure);
    }

    // This method is only used to provide a short list of test facilities and won't be used in the production version.
    public Task<IReadOnlyCollection<Facility>> GetListAsync() =>
        Task.FromResult<IReadOnlyCollection<Facility>>(FacilityData.GetData.OrderBy(facility => facility.Id).ToList());
}
