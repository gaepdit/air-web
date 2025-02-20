using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementActionService
{
    Task<Guid> CreateAsync(int caseFileId, CreateEnforcementActionDto resource,
        CancellationToken token = default);

    Task IssueAsync(Guid id, MaxCurrentDateOnlyDto dateDto, CancellationToken token = default);
    Task CancelAsync(Guid id, CancellationToken token);
    Task DeleteAsync(Guid id, CancellationToken token);
}
