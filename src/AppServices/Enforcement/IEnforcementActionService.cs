using AirWeb.AppServices.Enforcement.EnforcementActionCommand;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementActionService
{
    Task<Guid> CreateAsync(int caseFileId, CreateEnforcementActionDto resource,
        CancellationToken token = default);
}
