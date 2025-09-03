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
    public async Task<List<AnnualFeeSummary>> GetAnnualFeesAsync(FacilityId facilityId, DateOnly cutoffDate,
        int lookbackYears, bool forceRefresh = false)
    {
        if (!await facilityService.ExistsAsync(facilityId)) return [];

        var upperYear = cutoffDate.Month < 10 ? cutoffDate.Year - 1 : cutoffDate.Year;
        var lowerYear = upperYear - lookbackYears + 1;

        var cacheKey = AnnualFeesSummaryCacheKey(facilityId, lowerYear, upperYear);
        var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(CacheConstants.FeesSummaryExpiration);

        // Check the cache first.
        if (!forceRefresh &&
            cache.TryGetValue(cacheKey, out List<AnnualFeeSummary>? cachedAnnualFees) &&
            cachedAnnualFees != null)
        {
            logger.LogCacheHit(cacheKey);
            return cachedAnnualFees;
        }

        using var db = dbf.Create();

        var feesSummary = (await db.QueryAsync<AnnualFeeSummary>("air.GetIaipAnnualFeesSummary",
            param: new { FacilityId = facilityId.Id, LowerYear = lowerYear, UpperYear = upperYear },
            commandType: CommandType.StoredProcedure)).ToList();

        cache.Set(cacheKey, feesSummary, cacheOptions);
        logger.LogCacheRefresh(cacheKey, forceRefresh);

        return feesSummary;
    }

    private static string AnnualFeesSummaryCacheKey(FacilityId facilityId, int lowerYear, int upperYear) =>
        $"AnnualFeesHistory.{facilityId}.{lowerYear}.{upperYear}";
}
