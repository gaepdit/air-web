using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Reports.StackTestDto;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Reports;

public interface IReportsService
{
    Task<AccViewDto?> GetAccReportAsync(int id);
    Task<FceViewDto?> GetFceReportAsync(int id);
    Task<BaseStackTestReport?> GetStackTestReportAsync(FacilityId facilityId, int referenceNumber);
}
