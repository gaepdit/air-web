using AirWeb.AppServices.Compliance.AppNotifications;
using AirWeb.AppServices.Compliance.Comments;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Search;
using AirWeb.AppServices.Compliance.Compliance.Fces.SupportingData;
using AirWeb.AppServices.Compliance.Enforcement.Search;
using AirWeb.AppServices.Core.AppNotifications;
using AirWeb.AppServices.Core.Caching;
using AirWeb.AppServices.Core.CommonDtos;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Compliance.ComplianceEntities.Fces;
using AirWeb.Domain.Compliance.EnforcementEntities.CaseFiles;
using AutoMapper;
using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using IaipDataService.Facilities;
using IaipDataService.PermitFees;
using IaipDataService.SourceTests;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace AirWeb.AppServices.Compliance.Compliance.Fces;

#pragma warning disable S107 // Methods should not have too many parameters
public sealed class FceService(
    IMapper mapper,
    IFceRepository fceRepository,
    IFceManager fceManager,
    IComplianceWorkRepository complianceRepository,
    ICaseFileRepository caseFileRepository,
    IFacilityService facilityService,
    ISourceTestService sourceTestService,
    IPermitFeesService permitFeesService,
    IFceCommentService commentService,
    IUserService userService,
    IAppNotificationService appNotificationService,
    IMemoryCache cache,
    ILogger<FceService> logger) : IFceService
#pragma warning restore S107
{
    public async Task<FceViewDto?> FindAsync(int id, CancellationToken token = default)
    {
        var fce = mapper.Map<FceViewDto?>(await fceRepository.FindWithDetailsAsync(id, token).ConfigureAwait(false));
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

    private static readonly TimeSpan FceSupportingDataCacheTime = TimeSpan.FromDays(2);

    public async Task<SupportingDataPrintout> GetSupportingPrintoutDataAsync(FacilityId facilityId,
        DateOnly completedDate, CancellationToken token = default)
    {
        // Check the cache first.
        var cacheKey = $"FceSupportingPrintout.{facilityId}.{completedDate:yyyy-MM-dd}";
        if (cache.TryGetValue(cacheKey, logger, out SupportingDataPrintout? cachedValue))
            return cachedValue;

        var summary = new SupportingDataPrintout
        {
            Accs = await complianceRepository.GetListAsync<AccSummaryDto, AnnualComplianceCertification>(
                FilterFor<AnnualComplianceCertification>(), mapper, token: token).ConfigureAwait(false),

            Inspections = await complianceRepository.GetListAsync<InspectionSummaryDto, Inspection>(
                FilterFor<Inspection>(), mapper, token: token).ConfigureAwait(false),

            Notifications = await complianceRepository.GetListAsync<NotificationSummaryDto, Notification>(
                FilterFor<Notification>(), mapper, token: token).ConfigureAwait(false),

            Reports = await complianceRepository.GetListAsync<ReportSummaryDto, Report>(
                FilterFor<Report>(), mapper, token: token).ConfigureAwait(false),

            RmpInspections = await complianceRepository.GetListAsync<InspectionSummaryDto, RmpInspection>(
                FilterFor<RmpInspection>(), mapper, token: token).ConfigureAwait(false),

            SourceTests = await complianceRepository.GetListAsync<SourceTestSummaryDto, SourceTestReview>(
                FilterFor<SourceTestReview>(), mapper, token: token).ConfigureAwait(false),

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

        return cache.Set(summary, cacheKey, FceSupportingDataCacheTime, logger);

        Expression<Func<TSource, bool>> FilterFor<TSource>() where TSource : ComplianceWork =>
            source => source.FacilityId == facilityId && source.EventDate <= completedDate &&
                      source.EventDate >= completedDate.AddYears(-Fce.DataPeriod);
    }

    public async Task<SupportingDataDetails> GetSupportingDetailsAsync(FacilityId facilityId, DateOnly completedDate,
        bool forceRefresh = false, CancellationToken token = default)
    {
        // Check the cache first.
        var cacheKey = $"FceSupportingDetails.{facilityId}.{completedDate:yyyy-MM-dd}";
        var printoutCacheKey = $"FceSupportingPrintout.{facilityId}.{completedDate:yyyy-MM-dd}";

        if (forceRefresh) cache.RemoveAll([cacheKey, printoutCacheKey]);
        else if (cache.TryGetValue(cacheKey, logger, out SupportingDataDetails? cachedValue))
            return cachedValue;

        var complianceSpec = new ComplianceWorkSearchDto
        {
            FacilityId = facilityId,
            EventDateTo = completedDate,
            EventDateFrom = completedDate.AddYears(-Fce.DataPeriod),
        };
        var compliancePagination = new PaginatedRequest(pageNumber: 1, pageSize: 100,
            sorting: ComplianceWorkSortBy.WorkTypeAsc.GetDescription());

        var caseFileSpec = new CaseFileSearchDto
        {
            FacilityId = facilityId,
            EnforcementDateTo = completedDate,
            EnforcementDateFrom = completedDate.AddYears(-Fce.ExtendedDataPeriod),
        };
        var caseFilePaging = new PaginatedRequest(pageNumber: 1, pageSize: 100,
            sorting: CaseFileSortBy.IdAsc.GetDescription());

        var details = new SupportingDataDetails
        {
            AnnualFeesSummary = await permitFeesService
                .GetAnnualFeesAsync(facilityId, completedDate, Fce.ExtendedDataPeriod).ConfigureAwait(false),
            CaseFileSummary = await caseFileRepository
                .GetPagedListAsync<CaseFileSearchResultDto>(CaseFileFilters.SearchPredicate(caseFileSpec),
                    caseFilePaging, mapper, token: token).ConfigureAwait(false),
            ComplianceSummary = await complianceRepository
                .GetPagedListAsync<ComplianceWorkSearchResultDto>(ComplianceWorkFilters.SearchPredicate(complianceSpec),
                    compliancePagination, mapper, token: token).ConfigureAwait(false),
        };

        return cache.Set(details, cacheKey, FceSupportingDataCacheTime, logger);
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
            .SendNotificationAsync(FceTemplate.FceCreated, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);

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
            .SendNotificationAsync(FceTemplate.FceUpdated, fce.ReviewedBy, token, id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> DeleteAsync(int id, NotesDto resource,
        CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Delete(fce, resource.Notes, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(FceTemplate.FceDeleted, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public async Task<CommandResult> RestoreAsync(int id, CancellationToken token = default)
    {
        var fce = await fceRepository.GetAsync(id, token: token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);

        fceManager.Restore(fce, currentUser);
        await fceRepository.UpdateAsync(fce, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService
            .SendNotificationAsync(FceTemplate.FceRestored, fce.ReviewedBy, token, fce.Id).ConfigureAwait(false);
        return CommandResult.Create(notificationResult.FailureReason);
    }

    public Task<bool> ExistsAsync(FacilityId facilityId, int year, int currentId, CancellationToken token = default) =>
        fceRepository.ExistsAsync(facilityId, year, currentId, token);

    public Task<bool> ExistsAsync(int id, CancellationToken token = default) =>
        fceRepository.ExistsAsync(id, token);

    public async Task<CreateResult<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default)
    {
        var result = await commentService.AddCommentAsync(itemId, resource, token)
            .ConfigureAwait(false);

        var fce = await fceRepository.GetAsync(resource.ItemId, token: token).ConfigureAwait(false);

        var notificationResult = await appNotificationService.SendNotificationAsync(FceTemplate.FceCommentAdded,
            fce.ReviewedBy, token, itemId, resource.Comment, result.CommentUser?.FullName).ConfigureAwait(false);
        return CreateResult<Guid>.Create(result.CommentId, notificationResult.FailureReason);
    }

    public Task DeleteCommentAsync(Guid commentId, CancellationToken token = default) =>
        commentService.DeleteCommentAsync(commentId, token);

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => fceRepository.Dispose();
    public async ValueTask DisposeAsync() => await fceRepository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
