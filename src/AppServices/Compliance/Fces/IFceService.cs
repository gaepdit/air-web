﻿using AirWeb.AppServices.AppNotifications;
using AirWeb.AppServices.Comments;
using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Compliance.Fces.SupportingData;
using IaipDataService.Facilities;

namespace AirWeb.AppServices.Compliance.Fces;

public interface IFceService : IDisposable, IAsyncDisposable
{
    // Query
    Task<FceViewDto?> FindAsync(int id, CancellationToken token = default);
    Task<FceSummaryDto?> FindSummaryAsync(int id, CancellationToken token = default);
    Task<SupportingDataSummary> GetSupportingDataAsync(FacilityId facilityId, CancellationToken token = default);

    // Command
    Task<NotificationResultWithId<int>> CreateAsync(FceCreateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> UpdateAsync(int id, FceUpdateDto resource, CancellationToken token = default);
    Task<AppNotificationResult> DeleteAsync(int id, CommentDto resource, CancellationToken token = default);
    Task<AppNotificationResult> RestoreAsync(int id, CancellationToken token = default);
    Task<bool> ExistsAsync(FacilityId facilityId, int year, int currentId, CancellationToken token = default);

    // Comments
    Task<NotificationResultWithId<Guid>> AddCommentAsync(int itemId, CommentAddDto resource,
        CancellationToken token = default);

    Task DeleteCommentAsync(Guid commentId, CancellationToken token = default);
}
