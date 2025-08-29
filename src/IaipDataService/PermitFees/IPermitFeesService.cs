using IaipDataService.Facilities;

namespace IaipDataService.PermitFees;

public interface IPermitFeesService
{
    Task<List<AnnualFeeSummary>> GetAnnualFeesHistoryAsync(FacilityId facilityId, DateOnly cutoffDate,
        int lookbackYears, bool forceRefresh = false);
}
