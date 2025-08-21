using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Enforcement.EnforcementActionCommand;
using AirWeb.AppServices.Enforcement.EnforcementActionQuery;
using AirWeb.Domain.EnforcementEntities.EnforcementActions;

namespace AirWeb.AppServices.Enforcement;

public interface IEnforcementActionService : IDisposable, IAsyncDisposable
{
    // Create
    Task<Guid> CreateAsync(int caseFileId, EnforcementActionCreateDto resource, CancellationToken token = default);
    Task<Guid> CreateAsync(int caseFileId, ConsentOrderCommandDto resource, CancellationToken token = default);
    Task<Guid> CreateAsync(int caseFileId, AdministrativeOrderCommandDto resource, CancellationToken token = default);

    // Find
    Task<IActionViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<EnforcementActionType?> GetEnforcementActionType(Guid id, CancellationToken token = default);
    Task<CoViewDto> GetConsentOrderAsync(Guid id, CancellationToken token = default);

    // Update
    Task UpdateAsync(Guid id, EnforcementActionEditDto resource, CancellationToken token = default);
    Task UpdateAsync(Guid id, ConsentOrderCommandDto resource, CancellationToken token = default);
    Task UpdateAsync(Guid id, AdministrativeOrderCommandDto resource, CancellationToken token = default);

    // Other actions
    Task AddResponse(Guid id, MaxDateAndCommentDto resource, CancellationToken token = default);
    Task<bool> IssueAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token = default);
    Task CancelAsync(Guid id, CancellationToken token);
    Task ExecuteOrderAsync(Guid id, MaxDateOnlyDto resource, CancellationToken token);
    Task AppealOrderAsync(Guid id, MaxDateOnlyDto resource, CancellationToken token);
    Task<bool> ResolveAsync(Guid id, MaxDateAndBooleanDto resource, CancellationToken token);
    Task DeleteAsync(Guid id, CancellationToken token);
    Task AddStipulatedPenalty(Guid id, StipulatedPenaltyAddDto resource, CancellationToken token);
    Task DeletedStipulatedPenalty(Guid id, Guid stipulatedPenaltyId, CancellationToken token);

    // Reviews
    Task RequestReviewAsync(Guid id, EnforcementActionRequestReviewDto resource, CancellationToken token);
    Task SubmitReviewAsync(Guid id, EnforcementActionSubmitReviewDto resource, CancellationToken token);
}
