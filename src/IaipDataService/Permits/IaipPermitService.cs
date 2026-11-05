using Dapper;
using IaipDataService.Caching;
using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System.Data;

namespace IaipDataService.Permits;

public sealed class IaipPermitService(IDbConnectionFactory dbf, HybridCache cache, ILogger<IaipPermitService> logger)
    : IPermitService
{
    private async Task<IReadOnlyCollection<PermitSummary>> SearchPermitsByFacilityIdInternal(FacilityId facilityId,
        int skip, int take)
    {
        using var db = dbf.Create();
        return (await db.QueryAsync<PermitSummary>("air.SearchIaipPermitsByFacilityId",
            param: new { facilityId = facilityId.FormattedId, skip, take },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false)).ToList();
    }

    public async Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(FacilityId facilityId, int skip, int take,
        CancellationToken token = default)
    {
        var key = $"SearchPermitsByFacilityId.{facilityId}|skip:{skip}|take:{take}";
        var tag = $"IaipFacility.{facilityId}";
        logger.LogCacheSearch(key);

        return await cache.GetOrCreateAsync(key, factory: async _ =>
            {
                logger.LogCacheMiss(key);
                return await SearchPermitsByFacilityIdInternal(facilityId, skip, take).ConfigureAwait(false);
            },
            CacheUtilities.GetHybridCacheOptions(CacheConstants.FacilityExpiration),
            tags: [tag], token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(PermitSearchDto spec, int skip, int take,
        IPermitService.SortBy sortBy)
    {
        using var db = dbf.Create();
        return (await db.QueryAsync<PermitSummary>("air.SearchIaipPermits",
            param: new { spec.Name, spec.Permit, spec.DateFrom, spec.DateTo, skip, take, sort = sortBy.ToString() },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false)).ToList();
    }

    public async Task<int> CountPermitsAsync(FacilityId facilityId)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<int>("air.CountIaipPermitsByFacilityId",
            param: new { facilityId = facilityId.FormattedId },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    public async Task<int> CountPermitsAsync(PermitSearchDto spec)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<int>("air.CountIaipPermits",
            param: new { spec.Name, spec.Permit, spec.DateFrom, spec.DateTo },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    public async Task<byte[]?> GetPermitFileAsync(string fileName)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<byte[]?>("air.GetIaipPermitFile",
            param: new { fileName },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }
}
