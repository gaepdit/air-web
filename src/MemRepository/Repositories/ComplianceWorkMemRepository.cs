using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.Compliance;
using AirWeb.TestData.Lookups;

namespace AirWeb.MemRepository.Repositories;

public sealed class ComplianceWorkMemRepository()
    : BaseRepositoryWithMapping<ComplianceWork, int>(ComplianceWorkData.GetData), IComplianceWorkRepository
{
    // Local repository requires ID to be manually set.
    public int? GetNextId() => Items.Count == 0 ? 1 : Items.Select(work => work.Id).Max() + 1;

    public async Task<TWork?> FindAsync<TWork>(int id, bool includeExtras,
        CancellationToken token = default)
        where TWork : ComplianceWork =>
        (TWork?)await FindAsync(id, token: token).ConfigureAwait(false);

    public Task<ComplianceWorkType> GetComplianceWorkTypeAsync(int id, CancellationToken token = default) =>
        Task.FromResult(Items.Single(work => work.Id.Equals(id)).ComplianceWorkType);

    public Task<bool> SourceTestReviewExistsAsync(int referenceNumber, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<SourceTestReview>()
            .Any(str => str.ReferenceNumber == referenceNumber && !str.IsDeleted));

    public Task<SourceTestReview?> FindSourceTestReviewAsync(int referenceNumber, CancellationToken token = default) =>
        Task.FromResult(Items.OfType<SourceTestReview>()
            .FirstOrDefault(str => str.ReferenceNumber == referenceNumber && !str.IsDeleted));

    public Task<NotificationType> GetNotificationTypeAsync(Guid typeId, CancellationToken token = default) =>
        Task.FromResult(NotificationTypeData.GetData.Single(notificationType => notificationType.Id.Equals(typeId)));
}
