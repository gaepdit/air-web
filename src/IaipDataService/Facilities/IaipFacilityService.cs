using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Structs;
using IaipDataService.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Data;

namespace IaipDataService.Facilities;

public sealed class IaipFacilityService(
    IDbConnectionFactory dbf,
    IMemoryCache cache,
    ILogger<IaipFacilityService> logger) : IFacilityService
{
    public async Task<Facility> GetAsync(FacilityId id, bool forceRefresh = false)
    {
        var facility = await FindAsync(id, forceRefresh);
        if (facility is null) throw new InvalidOperationException("Facility not found.");
        return facility;
    }

    public async Task<Facility?> FindAsync(FacilityId? id, bool forceRefresh = false)
    {
        if (id is null || !await ExistsAsync(id)) return null;

        var cacheKey = $"IaipFacilityService.FindAsync.{id}";
        if (!forceRefresh && cache.TryGetValue(cacheKey, out Facility? cachedFacility) && cachedFacility != null)
        {
            logger.LogCacheHit(cacheKey);
            return cachedFacility;
        }

        logger.LogCacheRefresh(forceRefresh, cacheKey);

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
        facility.RegulatoryData!.Pollutants.AddRange(await multi.ReadAsync<Pollutant>());

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(IaipDataConstants.FacilityDataExpiration);
        cache.Set(cacheKey, facility, cacheEntryOptions);

        return facility;
    }

    public async Task<string> GetNameAsync(string id)
    {
        var facilityList = await GetListAsync();
        return facilityList.TryGetValue((FacilityId)id, out var name)
            ? name
            : throw new InvalidOperationException("Facility not found.");
    }

    public async Task<bool> ExistsAsync(FacilityId id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.IaipFacilityExists",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure);
    }

    private const string FacilityListCacheKey = "IaipFacilityService.GetListAsync";

    public async Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync(bool forceRefresh = false)
    {
        if (!forceRefresh &&
            cache.TryGetValue(FacilityListCacheKey, out ReadOnlyDictionary<FacilityId, string>? cachedFacilityList) &&
            cachedFacilityList != null)
        {
            logger.LogCacheHit(FacilityListCacheKey);
            return cachedFacilityList;
        }

        logger.LogCacheRefresh(forceRefresh, FacilityListCacheKey);

        using var db = dbf.Create();
        var facilityList = new ReadOnlyDictionary<FacilityId, string>(
            (await db.QueryAsync<KeyValuePair<FacilityId, string>>(
                sql: "air.GetIaipFacilityList", commandType: CommandType.StoredProcedure)).ToDictionary());

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(IaipDataConstants.FacilityListExpiration);
        cache.Set(FacilityListCacheKey, facilityList, cacheEntryOptions);

        return facilityList;
    }
}
