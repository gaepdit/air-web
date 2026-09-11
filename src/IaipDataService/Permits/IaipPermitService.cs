using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using System.Data;

namespace IaipDataService.Permits;

public class IaipPermitService(IDbConnectionFactory dbf) : IPermitService
{
    public async Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(FacilityId facilityId, int skip, int take)
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

    public async Task<IReadOnlyCollection<PermitSummary>> SearchPermitsAsync(string? name, string? permit, int skip,
        int take)
    {
        const string sql =
            "select FacilityId, FacilityName, PermitNumber, IssuanceDate, FileType, " +
            " VNarrative, VFinal, OtherNarrative, OtherPermit, " +
            " PSDAppSum, PSDPrelim, PSDNarrative, PSDFinalDet, PSDFinal " +
            " from dbo.VW_GA_PERMITS " +
            " where (@name is null or FacilityName like concat('%', @name, '%')) " +
            " and (@permit is null or PermitNumber like concat('%', @permit, '%')) " +
            " order by FacilityName, FacilityId, IssuanceDate, ApplicationNumber" +
            " offset @skip rows fetch next @take rows only ";

        using var db = dbf.Create();

        return (await db
            .QueryAsync<PermitSummary>(sql: sql, param: new { name, permit, skip, take }, commandType: CommandType.Text)
            .ConfigureAwait(false)).ToList();
    }

    public async Task<int> CountPermitsAsync(FacilityId facilityId)
    {
        const string sql = "select count(*) from dbo.VW_GA_PERMITS where FacilityId = @facilityId ";

        using var db = dbf.Create();

        return await db.ExecuteScalarAsync<int>(sql: sql, param: new { facilityId = facilityId.ToString() },
                commandType: CommandType.Text)
            .ConfigureAwait(false);
    }

    public async Task<int> CountPermitsAsync(string? name, string? permit)
    {
        const string sql =
            "select count(*) " +
            " from dbo.VW_GA_PERMITS " +
            " where (@name is null or FacilityName like concat('%', @name, '%')) " +
            " and (@permit is null or PermitNumber like concat('%', @permit, '%')) ";

        using var db = dbf.Create();

        return await db.ExecuteScalarAsync<int>(sql: sql, param: new { name, permit }, commandType: CommandType.Text)
            .ConfigureAwait(false);
    }

    public async Task<byte[]?> GetPermitFileAsync(string fileName)
    {
        const string sql = "SELECT PDFPERMITDATA FROM dbo.APBPERMITS WHERE STRFILENAME = @fileName ";

        using var db = dbf.Create();

        return await db.ExecuteScalarAsync<byte[]?>(sql: sql, param: new { fileName }, commandType: CommandType.Text)
            .ConfigureAwait(false);
    }
}
