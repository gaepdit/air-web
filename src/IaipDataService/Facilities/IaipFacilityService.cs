using Dapper;
using IaipDataService.Caching;
using IaipDataService.DbConnection;
using IaipDataService.Structs;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System.Data;

namespace IaipDataService.Facilities;

public sealed class IaipFacilityService(
    IDbConnectionFactory dbf,
    HybridCache cache,
    ILogger<IaipFacilityService> logger) : IFacilityService
{
    private const string FacilityLists = nameof(FacilityLists);

    public async Task<Facility?> FindFacilityAsync(FacilityId id, bool forceRefresh = false,
        CancellationToken token = default)
    {
        if (!await ExistsAsync(id).ConfigureAwait(false)) return null;

        var key = $"IaipFacilityDetails.{id}";
        var tag = $"IaipFacility.{id}";

        if (forceRefresh) await cache.RemoveByTagAsync(tags: [tag, FacilityLists], token).ConfigureAwait(false);
        else logger.LogCacheSearch(key);

        return await cache.GetOrCreateAsync(key, factory: async _ =>
            {
                if (forceRefresh) logger.LogCacheRefresh(key);
                else logger.LogCacheMiss(key);

                return await GetFacilityInternal(id).ConfigureAwait(false);
            },
            CacheUtilities.GetHybridCacheOptions(CacheConstants.FacilityExpiration),
            tags: [tag, FacilityLists], token).ConfigureAwait(false);
    }

    private async Task<Facility> GetFacilityInternal(FacilityId id)
    {
        using var db = dbf.Create();

        var multi = await db.QueryMultipleAsync("air.GetIaipFacilityDetails", param: new { FacilityId = id.Id },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
        await using var multiAsyncDisposable = multi.ConfigureAwait(false);

        var facility = multi.Read<Facility, Address, GeoCoordinates, RegulatoryData, Facility>(
            (facility, facilityAddress, geoCoordinates, regulatoryData) =>
            {
                facility.FacilityAddress = facilityAddress;
                facility.GeoCoordinates = geoCoordinates;
                facility.RegulatoryData = regulatoryData;
                return facility;
            }, splitOn: "FacilityAddressId,GeoCoordinatesId,RegulatoryDataId").Single();

        facility.RegulatoryData!.AirPrograms.AddRange(
            await multi.ReadAsync<AirProgram>().ConfigureAwait(false));
        facility.RegulatoryData!.ProgramClassifications.AddRange(
            await multi.ReadAsync<AirProgramClassification>().ConfigureAwait(false));
        facility.RegulatoryData!.Pollutants.AddRange(
            await multi.ReadAsync<Pollutant>().ConfigureAwait(false));

        return facility;
    }

    public async Task<string> GetNameAsync(string id) =>
        (await GetAllAsync().ConfigureAwait(false)).SingleOrDefault(f => f.FacilityId == id)?.Name ??
        throw new InvalidOperationException("Facility not found.");

    public async Task<bool> ExistsAsync(FacilityId id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.IaipFacilityExists",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    public async Task<ushort> GetNextActionNumberAsync(FacilityId id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<ushort>("air.GetIaipFacilityNextActionNumber",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    public async Task<DateTime?> GetFacilityEpaDxDateAsync(FacilityId id, CancellationToken token = default)
    {
        if (!await ExistsAsync(id).ConfigureAwait(false)) return null;

        var key = $"IaipFacilityEpaDxDate.{id}";
        var tag = $"IaipFacility.{id}";

        logger.LogCacheSearch(key);

        return await cache.GetOrCreateAsync(key, factory: async _ =>
            {
                logger.LogCacheMiss(key);
                return await GetFacilityEpaDxDateInternal(id).ConfigureAwait(false);
            },
            CacheUtilities.GetHybridCacheOptions(CacheConstants.FacilityExpiration),
            tags: [tag], token).ConfigureAwait(false);
    }

    private async Task<DateTime?> GetFacilityEpaDxDateInternal(FacilityId id)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<DateTime?>("etl.GetFacilityEpaDxDate",
            param: new { FacilityId = id.Id }, commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    public async Task<bool> RefreshEpaDataExchange(FacilityId id)
    {
        using var db = dbf.Create();

        var parameters = new DynamicParameters(new { AirsNumber = id.IaipDbId });
        parameters.Add("@returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

        await db.ExecuteAsync("etl.TriggerDataUpdateAtEPA", parameters, commandType: CommandType.StoredProcedure)
            .ConfigureAwait(false);

        return parameters.Get<int>("@returnValue") == 0;
    }

    public async Task<IReadOnlyCollection<FacilitySummary>> GetAllAsync(bool forceRefresh = false,
        bool includePortableSources = true, CancellationToken token = default)
    {
        var key = $"IaipFacilityList{(includePortableSources ? "" : "_ExcludingPortable")}";

        if (forceRefresh) await cache.RemoveByTagAsync(FacilityLists, token).ConfigureAwait(false);
        else logger.LogCacheSearch(key);

        return await cache.GetOrCreateAsync(key, factory: async _ =>
            {
                if (forceRefresh) logger.LogCacheRefresh(key);
                else logger.LogCacheMiss(key);

                return await GetFacilitySummariesFromDb(includePortableSources).ConfigureAwait(false);
            },
            CacheUtilities.GetHybridCacheOptions(CacheConstants.FacilityListExpiration),
            tags: [FacilityLists], token).ConfigureAwait(false);
    }

    private async Task<List<FacilitySummary>> GetFacilitySummariesFromDb(bool includePortableSources)
    {
        using var db = dbf.Create();

        return (await db.QueryAsync<FacilitySummary, GeoCoordinates, FacilitySummary>(
            sql: "air.GetIaipFacilityList",
            map: (facility, geoCoordinates) =>
            {
                facility.GeoCoordinates = geoCoordinates;
                return facility;
            },
            param: new { includePortableSources },
            splitOn: "GeoCoordinatesId",
            commandType: CommandType.StoredProcedure
        ).ConfigureAwait(false)).ToList();
    }
}
