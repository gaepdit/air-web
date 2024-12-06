using AirWeb.Domain.Reports.Compliance;
using AirWeb.Domain.Reports.StackTest;

namespace AirWeb.Domain.Reports;

public interface IReportsRepository
{
    Task<AccReport?> GetAccReportAsync(FacilityId facilityId, int id);
    Task<FceReport?> GetFceReportAsync(FacilityId facilityId, int id);
    Task<BaseStackTestReport?> GetStackTestReportAsync(FacilityId facilityId, int referenceNumber);
}
