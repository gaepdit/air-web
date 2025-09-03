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
    public Task<Facility?> FindFacilityDetailsAsync(FacilityId id, bool forceRefresh = false) =>
        GetFacilityAsync(id, forceRefresh, loadDetails: true);

    public Task<Facility?> FindFacilitySummaryAsync(FacilityId id, bool forceRefresh = false) =>
        GetFacilityAsync(id, forceRefresh, loadDetails: false);

    private async Task<Facility?> GetFacilityAsync(FacilityId id, bool forceRefresh, bool loadDetails)
    {
        if (!await ExistsAsync(id)) return null;

        var facilityDetailsCacheKey = FacilityDetailsCacheKey(id);
        var facilitySummaryCacheKey = FacilitySummaryCacheKey(id);
        var cacheKey = loadDetails ? facilityDetailsCacheKey : facilitySummaryCacheKey;

        if (!forceRefresh)
        {
            // When requesting a facility summary, check the summary cache first.
            if (!loadDetails && cache.TryGetValue(facilitySummaryCacheKey, logger, out Facility? cachedFacility))
                return cachedFacility;

            // Check the details cache when requesting facility details or
            // when requesting a summary that is not in the summary cache.
            if (cache.TryGetValue(facilityDetailsCacheKey, logger, out cachedFacility))
                return loadDetails
                    ? cachedFacility
                    : cache.Set(cacheKey, cachedFacility, CacheConstants.FacilityExpiration, logger);
        }

        using var db = dbf.Create();
        var spName = loadDetails ? "air.GetIaipFacilityDetails" : "air.GetIaipFacility";
        var varMultiTask = db.QueryMultipleAsync(spName, param: new { FacilityId = id.Id },
            commandType: CommandType.StoredProcedure);

        await using var multi = await varMultiTask;
        var facility = multi.Read<Facility, Address, GeoCoordinates, RegulatoryData, Facility>(
            (facility, facilityAddress, geoCoordinates, regulatoryData) =>
            {
                facility.FacilityAddress = facilityAddress;
                facility.GeoCoordinates = geoCoordinates;
                facility.RegulatoryData = regulatoryData;
                return facility;
            }, splitOn: "FacilityAddressId,GeoCoordinatesId,RegulatoryDataId").Single();

        if (loadDetails)
        {
            facility.RegulatoryData!.AirPrograms.AddRange(await multi.ReadAsync<AirProgram>());
            facility.RegulatoryData!.ProgramClassifications.AddRange(
                await multi.ReadAsync<AirProgramClassification>());
            facility.RegulatoryData!.Pollutants.AddRange(await multi.ReadAsync<Pollutant>());
        }

        return cache.Set(cacheKey, facility, CacheConstants.FacilityExpiration, logger, forceRefresh);
    }

    private static string FacilityDetailsCacheKey(FacilityId id) => $"IaipFacilityDetails.{id}";
    private static string FacilitySummaryCacheKey(FacilityId id) => $"IaipFacility.{id}";

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

    private const string FacilityListCacheKey = "IaipFacilityList";

    public async Task<ReadOnlyDictionary<FacilityId, string>> GetListAsync(bool forceRefresh = false)
    {
        if (!forceRefresh && cache.TryGetValue(FacilityListCacheKey, logger,
                out ReadOnlyDictionary<FacilityId, string>? cachedFacilityList))
            return cachedFacilityList;

        using var db = dbf.Create();
        var facilityList = new ReadOnlyDictionary<FacilityId, string>(
            (await db.QueryAsync<KeyValuePair<FacilityId, string>>(
                sql: "air.GetIaipFacilityList", commandType: CommandType.StoredProcedure)).ToDictionary());

        return cache.Set(FacilityListCacheKey, facilityList, CacheConstants.FacilityListExpiration, logger);
    }
}
