using AirWeb.AppServices.CommonDtos;
using AirWeb.AppServices.Notifications;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.Fces;
using AirWeb.Domain.ExternalEntities.Facilities;
using AutoMapper;

namespace AirWeb.AppServices.DomainEntities.Fces;

public sealed class FceService(
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    IMapper mapper,
    IUserService userService,
    IFceRepository fceRepository,
    IFceManager manager,
    IFacilityRepository facilityRepository)
    : IFceService
{
    public async Task<FceViewDto?> FindAsync(int id, CancellationToken token = default) =>
        mapper.Map<FceViewDto?>(await fceRepository.FindAsync(id, token).ConfigureAwait(false));

    public async Task<FceUpdateDto?> FindForUpdateAsync(int id, CancellationToken token = default) =>
        mapper.Map<FceUpdateDto?>(await fceRepository.FindAsync(fce => fce.Id.Equals(id) && !fce.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task<int> CreateAsync(FceCreateDto resource, CancellationToken token = default)
    {
        var facility = await facilityRepository.GetFacilityAsync(resource.FacilityId!, token).ConfigureAwait(false);
        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var fce = manager.Create(facility, resource.Year, currentUser);

        fce.ReviewedBy = await userService.FindUserAsync(resource.ReviewedById).ConfigureAwait(false);
        fce.CompletedDate = resource.CompletedDate;
        fce.OnsiteInspection = resource.OnsiteInspection;
        fce.Notes = resource.Notes;

        await fceRepository.InsertAsync(fce, token: token).ConfigureAwait(false);
        return fce.Id;
    }

    public async Task<NotificationResult> UpdateAsync(int id, FceUpdateDto resource, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<NotificationResult> AddCommentAsync(int id, AddCommentDto<int> resource,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<NotificationResult> DeleteAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<NotificationResult> RestoreAsync(ChangeEntityStatusDto<int> resource,
        CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose()
    {
        fceRepository.Dispose();
        facilityRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await fceRepository.DisposeAsync().ConfigureAwait(false);
        await facilityRepository.DisposeAsync().ConfigureAwait(false);
    }

    #endregion
}
