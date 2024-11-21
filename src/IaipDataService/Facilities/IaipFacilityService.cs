using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Structs;
using System.Collections.ObjectModel;
using System.Data;

namespace IaipDataService.Facilities;

public sealed class IaipFacilityService(IDbConnectionFactory dbf) : IFacilityService
{
    public async Task<Facility> GetAsync(FacilityId id)
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
        facility.RegulatoryData!.Pollutants = (await multi.ReadAsync<KeyValuePair<string, string>>()).ToDictionary();

        return facility;
    }

    public async Task<string> GetNameAsync(string id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<string>("air.GetIaipFacilityName",
                   param: new { FacilityId = ((FacilityId)id).Id }, commandType: CommandType.StoredProcedure) ??
               throw new InvalidOperationException("Facility not found.");
    }

    public async Task<bool> ExistsAsync(FacilityId id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.IaipFacilityExists",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync()
    {
        using var db = dbf.Create();
        return new ReadOnlyDictionary<FacilityId, string>((await db.QueryAsync<KeyValuePair<FacilityId, string>>(
            sql: "air.GetIaipFacilityList", commandType: CommandType.StoredProcedure)).ToDictionary());
    }
}
