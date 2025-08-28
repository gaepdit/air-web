using IaipDataService.Facilities;

namespace IaipDataService.PermitFees;

public interface IPermitFeesService
{
    Task<List<AnnualFeeSummary>> GetAnnualFeesHistoryForFacilityAsync(FacilityId facilityId, DateOnly cutoffDate,
        int lookbackYears, bool forceRefresh = false);
}
