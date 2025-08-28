using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using IaipDataService.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;

namespace IaipDataService.PermitFees;

public class IaipPermitFeesService(
    IFacilityService facilityService,
    IDbConnectionFactory dbf,
    IMemoryCache cache,
    ILogger<IaipPermitFeesService> logger) : IPermitFeesService
{
    public async Task<List<AnnualFeeSummary>> GetAnnualFeesHistoryForFacilityAsync(FacilityId facilityId,
        DateOnly cutoffDate, int lookbackYears, bool forceRefresh = false)
    {
        if (!await facilityService.ExistsAsync(facilityId)) return [];

        var upperYear = cutoffDate.Month < 10 ? cutoffDate.Year - 1 : cutoffDate.Year;
        var lowerYear = upperYear - lookbackYears + 1;

        var cacheKey = AnnualFeesHistoryCacheKey(facilityId, lowerYear, upperYear);
        var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(CacheConstants.FeesHistoryExpiration);

        // Check the cache first.
        if (!forceRefresh &&
            cache.TryGetValue(cacheKey, out List<AnnualFeeSummary>? cachedFeesHistory) &&
            cachedFeesHistory != null)
        {
            logger.LogCacheHit(cacheKey);
            return cachedFeesHistory;
        }

        using var db = dbf.Create();

        var feesSummary = (await db.QueryAsync<AnnualFeeSummary>("air.GetIaipAnnualFeesSummary",
            param: new { FacilityId = facilityId.Id, LowerYear = lowerYear, UpperYear = upperYear },
            commandType: CommandType.StoredProcedure)).ToList();

        cache.Set(cacheKey, feesSummary, cacheOptions);
        logger.LogCacheRefresh(cacheKey, forceRefresh);

        return feesSummary;
    }

    private static string AnnualFeesHistoryCacheKey(FacilityId facilityId, int lowerYear, int upperYear) =>
        $"AnnualFeesHistory.{facilityId}.{lowerYear}.{upperYear}";
}
