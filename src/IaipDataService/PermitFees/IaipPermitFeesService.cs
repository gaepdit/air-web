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
        int lookBackYears, bool forceRefresh = false)
    {
        if (!await facilityService.ExistsAsync(facilityId)) return [];

        var upperYear = cutoffDate.Month < 10 ? cutoffDate.Year - 1 : cutoffDate.Year;
        var lowerYear = upperYear - lookBackYears + 1;

        // Check the cache first.
        var cacheKey = $"AnnualFeesSummary.{facilityId}.{lowerYear}.{upperYear}";
        if (!forceRefresh && cache.TryGetValue(cacheKey, logger, out List<AnnualFeeSummary>? cachedValue))
            return cachedValue;

        using var db = dbf.Create();

        var feesSummary = (await db.QueryAsync<AnnualFeeSummary>("air.GetIaipAnnualFeesSummary",
            param: new { FacilityId = facilityId.Id, LowerYear = lowerYear, UpperYear = upperYear },
            commandType: CommandType.StoredProcedure)).ToList();

        return cache.Set(cacheKey, feesSummary, CacheConstants.FeesSummaryExpiration, logger);
    }
}
