using AirWeb.AppServices.Compliance.Fces;
using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.AppServices.Reports.ComplianceDto;
using AirWeb.AppServices.Reports.StackTestDto;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using IaipDataService.Facilities;
using IaipDataService.SourceTests;

namespace AirWeb.AppServices.Reports;

public class ReportsService(
    IWorkEntryService workEntryService,
    IFceService fceService,
    IFacilityService facilityService,
    ISourceTestService sourceTestService) : IReportsService
{
    public async Task<AccViewDto?> GetAccReportAsync(int id) => 
        await  workEntryService.FindAsync(id).ConfigureAwait(false) as AccViewDto;

    public Task<FceViewDto?> GetFceReportAsync(int id) => 
        fceService.FindAsync(id);

    public Task<BaseStackTestReport?> GetStackTestReportAsync(FacilityId facilityId, int referenceNumber)
    {
        throw new NotImplementedException();
    }
}
