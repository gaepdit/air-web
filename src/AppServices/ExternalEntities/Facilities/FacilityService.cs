using AirWeb.Domain.ExternalEntities.Facilities;
using AutoMapper;

namespace AirWeb.AppServices.ExternalEntities.Facilities;

public sealed class FacilityService(IMapper mapper, IFacilityRepository repository) : IFacilityService
{
    public async Task<FacilityViewDto?> FindAsync(FacilityId id, CancellationToken token = default) =>
        mapper.Map<FacilityViewDto>(await repository.FindFacilityAsync(id, token).ConfigureAwait(false));

    public Task<FacilityViewDto?> FindAsync(string? id, CancellationToken token = default) =>
        id is null || !FacilityId.IsValidFormat(id)
            ? Task.FromResult<FacilityViewDto?>(null)
            : FindAsync((FacilityId)id, token);

    public async Task<IReadOnlyCollection<FacilityViewDto>> GetListAsync(CancellationToken token = default) =>
        mapper.Map<IReadOnlyCollection<FacilityViewDto>>(await repository.GetListAsync(token).ConfigureAwait(false));

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync().ConfigureAwait(false);

    #endregion
}
