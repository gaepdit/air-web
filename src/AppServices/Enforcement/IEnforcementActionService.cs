using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementActionService
{
    Task<Guid> CreateAsync(int caseFileId, EnforcementActionCommandDto resource,
        CancellationToken token = default);

    Task<IActionViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default);

    Task AddResponse(Guid id, MaxDateAndCommentDto resource, CancellationToken token = default);
    Task<bool> IssueAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token = default);
    Task CancelAsync(Guid id, CancellationToken token);
    Task ExecuteOrderAsync(Guid id, MaxDateOnlyDto resource, CancellationToken token);
    Task AppealOrderAsync(Guid id, MaxDateOnlyDto resource, CancellationToken token);
    Task<bool> ResolveAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token);
    Task DeleteAsync(Guid id, CancellationToken token);

    Task UpdateAsync(Guid id, string notes, bool responseRequested, CancellationToken token = default);
    Task UpdateAsync(Guid id, AdministrativeOrderCommandDto resource, CancellationToken token = default);
    Task UpdateAsync(Guid id, ConsentOrderCommandDto resource, CancellationToken token = default);
}
