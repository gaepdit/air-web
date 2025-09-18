using IaipDataService.Facilities;

namespace IaipDataService.PermitFees;

public interface IPermitFeesService
{
    Task<List<AnnualFeeSummary>> GetAnnualFeesAsync(FacilityId facilityId, DateOnly cutoffDate, int lookbackYears,
        bool forceRefresh = false);
}
