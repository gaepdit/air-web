using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementActionService
{
    Task<Guid> CreateAsync(int caseFileId, CreateEnforcementActionDto resource,
        CancellationToken token = default);

    Task<IActionViewDto?> FindAsync(Guid id, CancellationToken token = default);

    Task AddResponse(Guid id, MaxDateAndCommentDto responseDto, CancellationToken token = default);
    Task IssueAsync(Guid id, MaxDateOnlyDto dateDto, CancellationToken token = default);
    Task CancelAsync(Guid id, CancellationToken token);
    Task ResolveAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token);
    Task DeleteAsync(Guid id, CancellationToken token);
}
