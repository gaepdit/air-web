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
        const string sql =
            "select FacilityId, FacilityName, PermitNumber, IssuanceDate, FileType, " +
            " VNarrative, VFinal, OtherNarrative, OtherPermit, " +
            " PSDAppSum, PSDPrelim, PSDNarrative, PSDFinalDet, PSDFinal " +
            " from dbo.VW_GA_PERMITS " +
            " where FacilityId = @facilityId " +
            " order by FacilityName, FacilityId, IssuanceDate, ApplicationNumber" +
            " offset @skip rows fetch next @take rows only ";

        using var db = dbf.Create();
        return (await db
            .QueryAsync<PermitSummary>(sql: sql, param: new { facilityId = facilityId.ToString(), skip, take },
                commandType: CommandType.Text)
            .ConfigureAwait(false)).ToList();
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

    public async Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(string? name, string? permit,
        DateOnly? dateFrom, DateOnly? dateTo, int skip, int take)
    {
        const string sql =
            "select FacilityId, FacilityName, PermitNumber, IssuanceDate, FileType, " +
            " VNarrative, VFinal, OtherNarrative, OtherPermit, " +
            " PSDAppSum, PSDPrelim, PSDNarrative, PSDFinalDet, PSDFinal " +
            " from dbo.VW_GA_PERMITS " +
            " where (@name is null or FacilityName like concat('%', @name, '%')) " +
            " and (@permit is null or PermitNumber like concat('%', @permit, '%')) " +
            " and (@dateFrom is null or IssuanceDate >= @dateFrom) " +
            " and (@dateTo is null or IssuanceDate <= @dateTo) " +
            " order by FacilityName, FacilityId, IssuanceDate, ApplicationNumber" +
            " offset @skip rows fetch next @take rows only ";

        using var db = dbf.Create();
        return (await db.QueryAsync<PermitSummary>(sql: sql, param: new { name, permit, dateFrom, dateTo, skip, take },
            commandType: CommandType.Text).ConfigureAwait(false)).ToList();
    }

    public async Task<int> CountPermitsAsync(FacilityId facilityId)
    {
        const string sql = "select count(*) from dbo.VW_GA_PERMITS where FacilityId = @facilityId ";
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<int>(sql: sql, param: new { facilityId = facilityId.ToString() },
            commandType: CommandType.Text).ConfigureAwait(false);
    }

    public async Task<int> CountPermitsAsync(string? name, string? permit, DateOnly? dateFrom, DateOnly? dateTo)
    {
        const string sql =
            "select count(*) " +
            " from dbo.VW_GA_PERMITS " +
            " where (@name is null or FacilityName like concat('%', @name, '%')) " +
            " and (@permit is null or PermitNumber like concat('%', @permit, '%')) " +
            " and (@dateFrom is null or IssuanceDate >= @dateFrom) " +
            " and (@dateTo is null or IssuanceDate <= @dateTo) ";

        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<int>(sql: sql, param: new { name, dateFrom, dateTo, permit },
            commandType: CommandType.Text).ConfigureAwait(false);
    }

    public async Task<byte[]?> GetPermitFileAsync(string fileName)
    {
        const string sql = "SELECT PDFPERMITDATA FROM dbo.APBPERMITS WHERE STRFILENAME = @fileName ";
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<byte[]?>(sql: sql, param: new { fileName }, commandType: CommandType.Text)
            .ConfigureAwait(false);
    }
}
