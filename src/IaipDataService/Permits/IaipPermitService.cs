using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using System.Data;

namespace IaipDataService.Permits;

public class IaipPermitService(IDbConnectionFactory dbf) : IPermitService
{
    public async Task<IReadOnlyCollection<PermitSummary>> GetPermitListAsync(string? facilityId, string? name,
        int skip, int take)
    {
        const string sql =
            "select FacilityId, FacilityName, PermitNumber, IssuanceDate, FileType, " +
            " VNarrative, VFinal, OtherNarrative, OtherPermit, " +
            " PSDAppSum, PSDPrelim, PSDNarrative, PSDFinalDet, PSDFinal " +
            " from dbo.VW_GA_PERMITS " +
            " where (@id is null or AIRSNumber = @id or AIRS = @id) " +
            "   and (@name is null or FacilityName like concat('%', @name, '%')) " +
            " order by FacilityName, FacilityId, IssuanceDate, ApplicationNumber" +
            " offset @skip rows fetch next @take rows only";

        var id = FacilityId.TryFormat(facilityId);

        using var db = dbf.Create();

        return (await db.QueryAsync<PermitSummary>(
            sql: sql,
            param: new { id, name, skip, take },
            commandType: CommandType.Text
        ).ConfigureAwait(false)).ToList();
    }

    public async Task<int> CountPermitsAsync(string? facilityId, string? name)
    {
        const string sql =
            "select count(*) " +
            " from dbo.VW_GA_PERMITS " +
            " where (@id is null or AIRSNumber = @id or AIRS = @id) " +
            "   and (@name is null or FacilityName like concat('%', @name, '%'))";

        using var db = dbf.Create();

        return await db.ExecuteScalarAsync<int>(
            sql: sql,
            param: new { id = facilityId, name },
            commandType: CommandType.Text
        ).ConfigureAwait(false);
    }

    public async Task<byte[]?> GetPermitFileAsync(string fileName)
    {
        const string sql = "SELECT PDFPERMITDATA FROM dbo.APBPERMITS WHERE STRFILENAME = @fileName";

        using var db = dbf.Create();

        return await db.ExecuteScalarAsync<byte[]?>(
            sql: sql,
            param: new { fileName },
            commandType: CommandType.Text
        ).ConfigureAwait(false);
    }
}
