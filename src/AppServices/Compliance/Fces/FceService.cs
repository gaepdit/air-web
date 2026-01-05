using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Caching;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using AirWeb.Domain.ComplianceEntities.ComplianceWork;
using AirWeb.Domain.ComplianceEntities.Fces;
using AirWeb.Domain.EnforcementEntities.CaseFiles;
using AutoMapper;
using IaipDataService.Facilities;
using IaipDataService.PermitFees;
using IaipDataService.SourceTests;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Fces;

#pragma warning disable S107 // Methods should not have too many parameters
public sealed class FceService(
    IMapper mapper,
    IFceRepository fceRepository,
    IFceManager fceManager,
    IWorkEntryRepository entryRepository,
    ICaseFileRepository caseFileRepository,
    IFacilityService facilityService,
    ISourceTestService sourceTestService,
    IPermitFeesService permitFeesService,
    ICommentService<int> commentService,
    IUserService userService,
    IAppNotificationService appNotificationService,
    IMemoryCache cache,
    ILogger<FceService> logger)
    : IFceService
#pragma warning restore S107
{
    public async Task<FceViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var fce = mapper.Map<FceViewDto?>(await fceRepository.FindWithExtrasAsync(id, token).ConfigureAwait(false));
        if (fce is null) return null;
        fce.FacilityName = await facilityService.GetNameAsync((FacilityId)fce.FacilityId).ConfigureAwait(false);
        return fce;
    }

    public async Task<FceSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default)
    {
        var fce = mapper.Map<FceSummaryDto?>(await fceRepository.FindAsync(id, token: token).ConfigureAwait(false));
        if (fce is null) return null;
        fce.FacilityName = await facilityService.GetNameAsync((FacilityId)fce.FacilityId).ConfigureAwait(false);
        return fce;
    }

    public async Task<SupportingDataSummary> GetSupportingDataAsync(FacilityId facilityId, DateOnly completedDate,
        CancellationToken token = default)
    {
        // Check the cache first.
        var cacheKey = $"FceSupportingData.{facilityId}.{completedDate:yyyy-MM-dd}";
        if (cache.TryGetValue(cacheKey, logger, out SupportingDataSummary? summary))
            return summary;

        summary = new SupportingDataSummary
        {
            Accs = await entryRepository.GetListAsync<AccSummaryDto, AnnualComplianceCertification>(
                For<AnnualComplianceCertification>(), mapper, token: token).ConfigureAwait(false),

            Inspections = await entryRepository.GetListAsync<InspectionSummaryDto, Inspection>(
                For<Inspection>(), mapper, token: token).ConfigureAwait(false),

            Notifications = await entryRepository.GetListAsync<NotificationSummaryDto, Notification>(
                For<Notification>(), mapper, token: token).ConfigureAwait(false),

            Reports = await entryRepository.GetListAsync<ReportSummaryDto, Report>(
                For<Report>(), mapper, token: token).ConfigureAwait(false),

            RmpInspections = await entryRepository.GetListAsync<InspectionSummaryDto, RmpInspection>(
                For<RmpInspection>(), mapper, token: token).ConfigureAwait(false),

            SourceTests = await entryRepository.GetListAsync<SourceTestSummaryDto, SourceTestReview>(
                For<SourceTestReview>(), mapper, token: token).ConfigureAwait(false),

            EnforcementCases = mapper.Map<IEnumerable<EnforcementCaseSummaryDto>>(
                await caseFileRepository.GetListAsync(caseFile =>
                        caseFile.FacilityId == facilityId &&
                        caseFile.EnforcementDate != null &&
                        caseFile.EnforcementDate >= completedDate.AddYears(-Fce.ExtendedDataPeriod) &&
                        caseFile.EnforcementDate <= completedDate,
                    includeProperties: [nameof(CaseFile.EnforcementActions)], token).ConfigureAwait(false)),

            Fees = await permitFeesService.GetAnnualFeesAsync(facilityId, completedDate, Fce.ExtendedDataPeriod)
                .ConfigureAwait(false),
        };

        await FillStackTestDataAsync(summary.SourceTests).ConfigureAwait(false);

        return cache.Set(cacheKey, summary, CacheConstants.FceSupportingData, logger);

        Expression<Func<TSource, bool>> For<TSource>() where TSource : WorkEntry => source =>
            source.FacilityId == facilityId && source.EventDate <= completedDate &&
            source.EventDate >= completedDate.AddYears(-Fce.DataPeriod);
    }

    private async Task FillStackTestDataAsync(IEnumerable<SourceTestSummaryDto> tests)
    {
        foreach (var test in tests.Where(test => test.ReferenceNumber != null))
        {
            var summary = await sourceTestService.FindSummaryAsync(test.ReferenceNumber!.Value).ConfigureAwait(false);
            test.AddDetails(summary);
        }
    }

    public async Task<CreateResult<int>> CreateAsync(FceCreateDto resource,
        CancellationToken token = default)
    {
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var fce = await fceManager.CreateAsync((FacilityId)resource.FacilityId!, resource.Year, currentUser)
            .ConfigureAwait(false);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById!).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        await fceRepository.InsertAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceCreated, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);

        return CreateResult<int>.Create(fce.Id, notificationResult.FailureReason);
    }

    public async Task<CommandResult> UpdateAsync(int id, FceUpdateDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById!).ConfigureAwait(false);
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes ?? string.Empty;

        fceManager.Update(fce, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceUpdated, fce.ReviewedBy, token, id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, CommentDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Delete(fce, resource.Comment, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceDeleted, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Restore(fce, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(Template.FceRestored, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int currentId, CancellationToken token = default) =>
        fceRepository.ExistsAsync(facilityId, year, currentId, token);

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(fceRepository, itemId, resource, token)
            .ConfigureAwait(false);

        var fce = await fceRepository.GetAsync(resource.ItemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService.SendNotificationAsync(Template.FceCommentAdded,
            fce.ReviewedBy, token, itemId, resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false);
        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(fceRepository, commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => fceRepository.Dispose();
    public async ValueTask DisposeAsync() => await fceRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
