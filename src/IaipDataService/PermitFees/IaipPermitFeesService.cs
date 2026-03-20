using Dapper;
using IaipDataService.DbConnection;
using IaipDataService.Facilities;
using System.Data;

namespace IaipDataService.PermitFees;

public class IaipPermitFeesService(IFacilityService facilityService, IDbConnectionFactory dbf) : IPermitFeesService
{
    public async Task<List<AnnualFeeSummary>> GetAnnualFeesAsync(FacilityId facilityId, DateOnly cutoffDate,
        int lookBackYears)
    {
        if (!await facilityService.ExistsAsync(facilityId).ConfigureAwait(false)) return [];

        var upperYear = cutoffDate.Month < 10 ? cutoffDate.Year - 1 : cutoffDate.Year;
        var lowerYear = upperYear - lookBackYears + 1;

        using var db = dbf.Create();

        return (await db.QueryAsync<AnnualFeeSummary>("air.GetIaipAnnualFeesSummary",
            param: new { FacilityId = facilityId.Id, LowerYear = lowerYear, UpperYear = upperYear },
            commandType: CommandType.StoredProcedure).ConfigureAwait(false)).ToList();
    }
}
